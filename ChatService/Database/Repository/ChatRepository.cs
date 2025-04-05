using System.Text;
using System.Text.Json;
using ChatService.Abstractions;
using ChatService.Database.Context;
using ChatService.Database.Entities;
using ChatService.Entities;
using ChatService.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace ChatService.Repository;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IConnection _rabbitMqConnection;
    private readonly ILogger<ChatRepository> _logger;

    public ChatRepository(
        AppDbContext dbContext,
        IConnection rabbitMqConnection,
        ILogger<ChatRepository> logger
    )
    {
        _dbContext = dbContext;
        _rabbitMqConnection = rabbitMqConnection;
        _logger = logger;
    }

    public async Task<string?> SendEvent(byte[] body)
    {
        await using var channel = await _rabbitMqConnection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync("invoked_event", ExchangeType.Fanout, durable: true);
        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent
        };
        await channel.BasicPublishAsync("invoked_event", "", false, properties, body);
        return null;
    }

    public async Task<List<ChatDto>> GetChatList(string userId)
{
    var userGuid = Guid.Parse(userId);
    var chatList = await _dbContext.ChatMembers
        .Where(m => m.UserId == userGuid)
        .Include(m => m.Chat)
            .ThenInclude(c => c.Members)
        .Select(m => new ChatDto
        {
            id = m.Chat.Id,
            name = m.Chat.Name,
            ownerId = m.Chat.OwnerId,
            isDm = m.Chat.isDm,
            members = m.Chat.Members.Select(member => member.UserId).ToList(),
        })
        .ToListAsync();
    var chatIds = chatList.Select(c => c.id).ToList();
    var lastMessages = await _dbContext.Messages
        .Where(msg => chatIds.Contains(msg.ChatId))
        .GroupBy(msg => msg.ChatId)
        .Select(group => group.OrderByDescending(msg => msg.Date).First())
        .ToListAsync();
    var lastReadIndexes = await _dbContext.MessageStatus
        .Where(ms => ms.UserId == userGuid && chatIds.Contains(ms.ChatId))
        .ToDictionaryAsync(ms => ms.ChatId, ms => ms.LastReaden);
    var messages = await _dbContext.Messages
        .Where(msg => chatIds.Contains(msg.ChatId) && msg.SenderId != userGuid)
        .ToListAsync();
    var unreadCounts = messages
        .GroupBy(msg => msg.ChatId)
        .ToDictionary(
            group => group.Key,
            group => group.Count(msg => !lastReadIndexes.TryGetValue(msg.ChatId, out int lastRead) || msg.MessageId > lastRead)
        );
    foreach (var chat in chatList)
    {
        var lastMessage = lastMessages.FirstOrDefault(msg => msg.ChatId == chat.id);
        if (lastMessage != null)
        {
            chat.lastMessage = new LastMessageDto
            {
                messageId = lastMessage.MessageId,
                senderId = lastMessage.SenderId,
                msg = lastMessage.Msg,
                date = lastMessage.Date,
                isFunctional = lastMessage.IsFunctional
            };
        }
        chat.unreadCount = unreadCounts.TryGetValue(chat.id, out int count) ? count : 0;
    }
    return chatList;
}

    public async Task<string?> CreateGroup(string userId, string groupName, List<string> users){
        if (string.IsNullOrWhiteSpace(groupName)) {
            return "Group name is required";
        }
        if (!users.Contains(userId)) {
            users.Add(userId);
        }
        var userIdsExist = await _dbContext.Users
            .Where(u => users.Contains(u.Id.ToString()))
            .Select(u => u.Id)
            .ToListAsync();
        var chat = new Chat { Name = groupName, Members = new List<ChatMember>(), CreatedAt = DateTime.UtcNow, OwnerId=Guid.Parse(userId) };
        foreach (var user in users) {
            chat.Members.Add(new ChatMember { UserId = Guid.Parse(user), Chat = chat });
        }
        await _dbContext.Chats.AddAsync(chat);
        var message = Message.ChatCreated(chat.Id, Guid.Parse(userId));
        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();
        await SendEvent(ChatAddedEvent.FromNewChat(chat, message.ToLastDto()).Serialize());
        return null;
    }

    public async Task<List<Message>> GetMsgs(string chatId, string userId, int startId, int amount, bool toNew) {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if (user == null) return new ();
        // check that user in chat
        var chat = await _dbContext.Chats
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id.ToString() == chatId);
        if (chat == null) return new ();
        if (!chat.Members.Any(m => m.UserId == user.Id)) return new ();

        var msgs = await _dbContext.Messages
            .Where(m => m.ChatId.ToString() == chatId)
            .Where(m => m.MessageId < (startId == -1 ? int.MaxValue : startId))
            .OrderBy(m => toNew ? m.MessageId : -m.MessageId)
            .Take(amount)
            .ToListAsync();
        foreach (var msg in msgs) {
            _logger.LogWarning($"Message: {msg.ChatId} {msg.Msg} {msg.Attachments}");
        }
        return msgs;
    }

    public async Task<Message?> SendMsg(string chatId, string userId, string msg, List<string> attachments) {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if (user == null) return null;
        var chat = await _dbContext.Chats
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id.ToString() == chatId);
        if (chat == null) return null;
        if (!chat.Members.Any(m => m.UserId == user.Id)) return null;
        var message = new Message
        {
            ChatId = chat.Id,
            SenderId = Guid.Parse(userId),
            Msg = msg,
            Date = DateTime.UtcNow,
            IsEdited = false,
            Attachments = attachments,
            IsFunctional = false
        };
        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();
        await SendEvent(NewMessageEvent.FromMessage(message).Serialize());
        return message;
    }

    public async Task<List<string>> GetChatMembers(string chatId) {
        _logger.LogWarning("Getting chat members for chatId: {ChatId}", chatId);
        var chat = await _dbContext.Chats
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id.ToString() == chatId);
        if (chat == null) return new ();
        return chat.Members.Select(m => m.UserId.ToString()).ToList();
    }

    public async Task<Chat?> StartDm(string initUserId, string userId, string message) {
        var user1 = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == initUserId);
        var user2 = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if (user1 == null || user2 == null) return null; // user1 == user2 is impossible
        // check dm existance
        var dm = await _dbContext.Chats
            .Where(c => c.isDm == true)
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Members.Any(m => m.UserId == user1.Id) && c.Members.Any(m => m.UserId == user2.Id));
        if (dm != null) return null;
        // create dm
        var chat = new Chat { Name = "", Members = new List<ChatMember>(), CreatedAt = DateTime.UtcNow, OwnerId=Guid.Parse(initUserId), isDm=true };
        chat.Members.Add(new ChatMember { UserId = Guid.Parse(initUserId), Chat = chat });
        chat.Members.Add(new ChatMember { UserId = Guid.Parse(userId), Chat = chat });
        await _dbContext.Chats.AddAsync(chat);
        await _dbContext.Messages.AddAsync(Message.ChatCreated(chat.Id, Guid.Parse(userId)));
        var msg = new Message{
            ChatId = chat.Id,
            SenderId = Guid.Parse(initUserId),
            Msg = message,
            Date = DateTime.UtcNow,
            IsEdited = false,
            Attachments = null,
            IsFunctional = false
        };
        await _dbContext.Messages.AddAsync(msg);
        await _dbContext.SaveChangesAsync();
        await SendEvent(ChatAddedEvent.FromNewChat(chat, msg.ToLastDto()).Serialize());
        return chat;
    }

    public async Task<string?> LeaveGroup(string chatId, string userId) {
        var chat = await _dbContext.Chats
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id.ToString() == chatId);
        if (chat == null) return "No such chat!";
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if (user == null) return "No such user!";
        if (!chat.Members.Any(m => m.UserId == user.Id)) return "You are not in this chat!";
        if (chat.isDm) return "You can't leave dm!";
        chat.Members.Remove(chat.Members.First(m => m.UserId == user.Id));
        if (chat.Members.Count == 0) {
            _dbContext.Chats.Remove(chat);
            await _dbContext.SaveChangesAsync();
        } else {
            var msg = Message.UserLeaved(chat.Id, user.Id);
            await _dbContext.Messages.AddAsync(msg);
            await _dbContext.SaveChangesAsync();
            await SendEvent(NewMessageEvent.FromMessage(msg).Serialize());
        }
        await SendEvent(ChatRemovedEvent.FromChatId(chatId, [userId]).Serialize());
        return null;
    }

    public async Task<string?> DeleteChat(string chatId, string userId) {
        _logger.LogWarning("Deleting chat: {ChatId}", chatId);
        var chat = await _dbContext.Chats
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id.ToString() == chatId);
        _logger.LogWarning("Chat: {Chat}", chat);
        if (chat == null) return "Chat not found";
        if (!(chat.isDm && chat.Members.Any(m => m.UserId.ToString() == userId)) && (chat.OwnerId.ToString() != userId)) return "You are not chat owner";
        var members = chat.Members.Select(m => m.UserId.ToString()).ToList();
        var msgs = await _dbContext.Messages.Where(m => m.ChatId.ToString() == chatId).ToListAsync();
        _dbContext.Chats.Remove(chat);
        _dbContext.Messages.RemoveRange(msgs);
        await _dbContext.SaveChangesAsync();
        _logger.LogWarning("Chat removed: {ChatId}", chatId);
        _logger.LogWarning("Chat members: {Members}", members);
        await SendEvent(ChatRemovedEvent.FromChatId(chatId, members).Serialize());
        return null;
    }

    public async Task<ChatDto?> AddToGroup(string invokerId, string chatId, string userId) {
        var invoker = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == invokerId);
        var chat = await _dbContext.Chats
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id.ToString() == chatId);
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if (invoker == null || chat == null || user == null || chat.isDm ||
        chat.Members.Any(m => m.UserId == user.Id) || !chat.Members.Any(m => m.UserId == invoker.Id)) return null;
        chat.Members.Add(new ChatMember { UserId = user.Id, Chat = chat });
        var msg = Message.UserAdded(chat.Id, user.Id, invokerId);
        await _dbContext.Messages.AddAsync(msg);
        await _dbContext.SaveChangesAsync();
        await SendEvent(NewMessageEvent.FromMessage(msg).Serialize());
        return ChatDto.FromChatLast(chat, msg.ToLastDto());
    }

    public async Task ReadMessages(string chatId, string userId, int lastMessageId) {
        var chat = await _dbContext.Chats
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id.ToString() == chatId);
        if (chat == null) return; 
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if (user == null) return; 
        if (!chat.Members.Any(m => m.UserId == user.Id)) return;
        var last = lastMessageId;
        if (lastMessageId == -1) {
            last = await _dbContext.Messages.Where(m => m.ChatId.ToString() == chatId && m.SenderId != user.Id).MaxAsync(m => m.MessageId);
        }
        var existingStatus = await _dbContext.MessageStatus
            .FirstOrDefaultAsync(ms => ms.UserId == user.Id && ms.ChatId == Guid.Parse(chatId));
        if (existingStatus != null){existingStatus.LastReaden = last;}
        else{
            _dbContext.MessageStatus.Add(new MessageStatus { LastReaden = last, UserId = user.Id, ChatId = Guid.Parse(chatId) });
        }
        await _dbContext.SaveChangesAsync();
        var unreadCount = await _dbContext.Messages.Where(m => m.ChatId.ToString() == chatId && m.SenderId != user.Id && m.MessageId > last).CountAsync();
        await SendEvent(new MessagesReadenEvent {
            chatId = chatId,
            unreadCount = unreadCount,
            userId = userId,
            lastMessageId = last
        }.Serialize());
    }

    public async Task<string?> DeleteMessage(int messageId, string userId) {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if (user == null) return "User not found";
        var msg = await _dbContext.Messages.FirstOrDefaultAsync(m => m.MessageId == messageId);
        if (msg == null) return "Message not found";
        if (msg.SenderId != user.Id) return $"You can't delete this message! {msg.MessageId} {msg.Msg} {msg.SenderId} {user.Id} {userId}";
        _dbContext.Messages.Remove(msg);
        await _dbContext.SaveChangesAsync();
        await SendEvent(new MessageDeletedEvent{
            chatId = msg.ChatId.ToString(),
            messageId = messageId
        }.Serialize());
        return null;
    }

    public async Task<string?> EditMessage(int messageId, string newMessage, string userId) {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if (user == null) return "User not found";
        var msg = await _dbContext.Messages.FirstOrDefaultAsync(m => m.MessageId == messageId);
        if (msg == null) return "Message not found";
        if (msg.SenderId != user.Id) return "You can't edit this message";
        msg.Msg = newMessage;
        msg.IsEdited = true;
        await _dbContext.SaveChangesAsync();
        await SendEvent(new MessageEditedEvent {
            chatId = msg.ChatId.ToString(),
            senderId = msg.SenderId.ToString(),
            messageId = messageId,
            newMessage = newMessage
        }.Serialize());
        return null;
    }

    public async Task<string?> RenameGroup(string chatId, string name, string userId) {
        var chat = await _dbContext.Chats
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id.ToString() == chatId);
        if (chat == null) return "Chat not found";
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if (user == null) return "User not found";
        if (chat.isDm) return "You can't rename dm";
        if (!chat.Members.Any(m => m.UserId == user.Id)) return "You are not in this chat";
        chat.Name = name;
        var msg = Message.GroupRenamed(chat.Id, Guid.Parse(userId), name);
        await _dbContext.Messages.AddAsync(msg);
        await _dbContext.SaveChangesAsync();
        await SendEvent(NewMessageEvent.FromMessage(msg).Serialize());
        await SendEvent(new GroupRenamedEvent {
            chatId = chatId,
            newName = name
        }.Serialize());
        return null;
    }
}



