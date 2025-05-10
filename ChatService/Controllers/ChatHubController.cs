using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using ChatService.Abstractions;
using ChatService.CustomSwaggerGen;
using Cogni.Database.Context;
using Cogni.Authentication;
using Cogni.Database.Entities;
using ChatService.Repository;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace ChatService.Controllers;

public class ChatHubController : Hub
{
    private static readonly ConcurrentDictionary<string, string> ConnectionToUser = new();
    private static readonly ConcurrentDictionary<string, HashSet<string>> UserToConnections = new();
    private readonly ILogger<ChatHubController> _logger;
    private readonly IHubService _hubService;
    private readonly IChatRepository _db;
    private readonly IRedisRepository _redisRepository;
    private readonly TokenValidation _tokenValidation;
    public ChatHubController(
        ILogger<ChatHubController> logger,
        IHubService hubService,
        IRedisRepository redis,
        IChatRepository db,
        IConfiguration config
    ) {
        _logger = logger;
        _hubService = hubService;
        _redisRepository = redis;
        _db = db;
        _tokenValidation = new TokenValidation(config);
    }

    [InvokableSignalREvent("Connection root"),
    SignalRCustomEventDescription("You must add \"userId\" to params;\tЕсли инъекция js сработала, то этот метод этого ивента - CONN, значит и остальные методы, связанные с signalR тоже заменятся. Если этого не случилось - ориентируйтесь по пути перед названием эндпоинта. /invoke/Event - INVOKE, /invoke-response/Event - RESPONSE, /listen - LISTEN. Объяснение API: [Методы] Тут все работает на SignalR - обертке над Websockets. Все ответы приходят в json-формате. Мною созданы три типа ивентов, все они ниже: INVOKE - те, которые вы можете вызывать с клиента; RESPONSE - негарантированный ответ после INVOKE. LISTEN - те, которые вы можете слушать на клиенте (как и RESPONSE, кстати). Ниже описаны все эти ивенты. Если что-то не понятно - есть мой ChatDevFrontend для демонстрации работы. Теперь к параметрам: У INVOKE при вызове со стороны клиента важна последовательность, а имена указать не получится, будьте внимательны! У RESPONSE параметры это описание ответа в json - формате, там простые типы - дтошка, сущность, словарик или массив. У LISTEN - Тоже описание ответа, но уже сложнее, все представленные параметры - \"ключи\" json, то есть если указаты параметры chatId (string) и senderId (string), то json будет {chatId: \"string\", senderId: \"string\"}. На момент написания все LISTEN затрагивают одно конкретное изменение одного конкретного элемента. [Чаты] Чаты бывают двух видов: группы и личные сообщения (лс/direct/dm). Я специально разделил понятия chat и group: chat включает в себя как лс, так и группы, а group только группы. Таким образом проще понять с чем ожидается взаимодействие. [Ошибки] Я сделал ErrorResponse для того, чтобы оповещать пользователя, что ожидаемое действие не было совершено. Далеко не все ошибки покрыты, так что добавляйте при надобности, вроде как все сделано довольно просто. [Nginx, Haproxy, Масштабируемость] Микросервис чатов сделан так, чтобы он легко масштабировался для балансировки нагрузки путем дубликации. Nginx'совские методы почему-то не хотят корректно распределять нагрузку между ними (ip_hash не работает в докере (хэширует докеровский ip), хэширование forwarded-for нестабильно, а реализация через авто-выдачу кук требует нестандартный nginx), поэтому я пересел на Haproxy."),
    EventWithResponse(
        "ErrorResponse",
        "When something went wrong",
        null,
        typeof(string),
        "Just a message describing what went wrong"
    )]
    public override async Task OnConnectedAsync()
    {
        _logger.LogWarning($"Connected: {Context.ConnectionId}");
        var token = Context.GetHttpContext()?.Request.Query["token"];
        _logger.LogWarning($"Token: {token}");
        try {
            var payload = _tokenValidation.GetTokenPayload(token);
            _logger.LogWarning($"Payload: {payload}");
            var userId = payload.UserId;
            _hubService.AddRel(userId.ToString(), Context.ConnectionId);
            await base.OnConnectedAsync();
        } catch (Exception e) {
            _logger.LogWarning($"Aborted due invalid token: {e.Message}");
            Context.Abort();
            return;
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogWarning($"Disconnected: {Context.ConnectionId}");
        _hubService.RemoveRel(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    [InvokableSignalREvent($"Used for notifying that client is online. Send every (" + RedisRepository.ONLINE_LIFETIME_SECONDS + " - 1) seconds.")]
    public async Task ImHere()
    {
        var user = _hubService.GetUserId(Context.ConnectionId);
        _logger.LogDebug($"ImHere: {user}");
        if (user == null) return;
        await _redisRepository.UserHere(user);
    }

    [InvokableSignalREvent("Used for requesting online users."),
    SignalRCustomEventDescription("Its very efficient if request multiple users at once. You can invoke it very offen, like every one second. Suggested request time: (" + RedisRepository.ONLINE_LIFETIME_SECONDS + " - 1) seconds."),
    EventArgDescription("userIds", "Array of userIds"),
    EventWithResponse(
        "UsersOnline",
        "Online status of users",
        null,
        typeof(Dictionary<string, bool>),
        "Dictionary of users. Key is userId, value is boolean represents online status."
    )]
    public async Task GetUsersOnline(string[] userIds)
    {
        _logger.LogDebug($"GetUsersOnline: {userIds}");
        var online = await _redisRepository.GetOnline(userIds);
        if (online.Count == 0) return;
        await Clients.Caller.SendAsync("UsersOnline", online);
    }

    [InvokableSignalREvent("Used for requesting chat list on init."),
    SignalRCustomEventDescription("Please use only on init"),
    EventWithResponse(
        "ChatList",
        "List of chats",
        null,
        typeof(List<Models.ChatDto>),
        "List of chat objects"
    )]
    public async Task GetChatList()
    {
        var user = _hubService.GetUserId(Context.ConnectionId);
        if (user == null) return;
        var chat_list = await _db.GetChatList(user);
        _logger.LogWarning($"Chat list: {chat_list}");
        if (chat_list.Count == 0){
            return;
        }
        await Clients.Caller.SendAsync("ChatList", chat_list);
    }

    [InvokableSignalREvent("Used for creating group."),
    EventArgDescription("groupName", "Name of group"),
    EventArgDescription("users", "List of members - userIds"),
    SignalRCustomEventDescription("Invoker will be added if not presented in users list. Invoker will be group owner.")]
    public async Task CreateGroup(string groupName, List<string> users) {
        var user = _hubService.GetUserId(Context.ConnectionId);
        if (user == null) return;
        var err = await _db.CreateGroup(user, groupName, users);
        if (err != null) {
            await Clients.Caller.SendAsync("ErrorResponse", err);
        }
    }

    [InvokableSignalREvent("Used for getting messages."),
    EventArgDescription("chatId", "Id of chat"),
    EventArgDescription("startId", "Id of message to start from"),
    EventArgDescription("toNew", "Does it go to new or old messages"),
    EventArgDescription("amount", "Amount of messages to get"),
    SignalRCustomEventDescription("Used for getting messages."),
    EventWithResponse(
        "Msgs",
        "List of messages",
        null,
        typeof(List<Cogni.Database.Entities.Message>),
        "Use -1 as startId to start from last message in chat. List of message objects, order isn't garanteed. Message id is ordered and isn't chat-unique. If you are getting empty list - there are no messages - that may be useful for stop requesting images when approach first message in chat."
    )]
    public async Task GetMsgs(string chatId, int startId, bool toNew, int amount) {
        _logger.LogWarning($"Getting msgs for chatId: {chatId} {startId} {amount}");
        var user = _hubService.GetUserId(Context.ConnectionId);
        if (user == null) return;
        var msgs = await _db.GetMsgs(chatId, user, startId, amount, toNew);
        await Clients.Caller.SendAsync("Msgs", msgs);
    }

    [InvokableSignalREvent("Used for sending messages."),
    EventArgDescription("chatId", "Id of chat"),
    EventArgDescription("msg", "Text of message"),
    EventArgDescription("attachments", "List of links to attach. Links may be domain-relative (like \"/chat-files/cf683044-98bd-4e35-9cdd-a8da0abb7278_cat.png\") or absolute (\"https://domain.tld/chat-files/cf683044-98bd-4e35-9cdd-a8da0abb7278_cat.png\"). Absolute images probably will only works on production - depends on frontend."),
    SignalRCustomEventDescription("Use response carefuly or you may will have duplicate messages!"),
    EventWithResponse(
        "MsgSent",
        "!USE CAREFULY! Response for sent message",
        "!USE CAREFULY! Sent message, just for speeding up user-response. Also NewMsg event will be sent, so, if you will use that respose without any client-side tracking mechanism, you will have duplicate messages. Message id is ordered and isn't chat-unique.",
        typeof(Cogni.Database.Entities.Message),
        "Message object"
    )]
    public async Task SendMsg(string chatId, string msg, List<string> attachments) {
        var user = _hubService.GetUserId(Context.ConnectionId);
        _logger.LogWarning($"Received message: {msg}");
        _logger.LogWarning($"Received message: '{msg}'");
        _logger.LogWarning($"Sending msg: {msg.Replace("\n", "\\n")}");
        if (user == null) return;
        if (attachments != null && attachments.Count > 9) {
            await Clients.Caller.SendAsync("ErrorResponse", "Too many attachments");
            return;
        }
        _logger.LogWarning($"Sending msg for chatId: {chatId} {msg} {attachments}");
        var message = await _db.SendMsg(chatId, user, msg, attachments);
        await _redisRepository.StopTyping(user, chatId);
        await Clients.Caller.SendAsync("MsgSent", message);
    }

    [InvokableSignalREvent("Used for creating direct."),
    EventArgDescription("userId", "Id of user"),
    EventArgDescription("msg", "Text of message"),
    SignalRCustomEventDescription("Users can only have one dm with another user.")]
    public async Task StartDm(string userId, string msg) {
        var user = _hubService.GetUserId(Context.ConnectionId);
        if (user == null) return;
        var chat = await _db.StartDm(user, userId, msg);
        if (chat == null){
            await Clients.Caller.SendAsync("ErrorResponse", $"Cant start a dm with {userId}!");
        }
    }

    [InvokableSignalREvent("Used for leaving group."),
    EventArgDescription("chatId", "Id of group"),
    SignalRCustomEventDescription("Users can't leave dms. Use DeleteChat instead.")]
    public async Task LeaveGroup(string chatId) {
        var user = _hubService.GetUserId(Context.ConnectionId);
        if (user == null) return;
        var err = await _db.LeaveGroup(chatId, user);
        if (err != null) {
            await Clients.Caller.SendAsync("ErrorResponse", err);
        }
    }

    [InvokableSignalREvent("Used for deleting group."),
    EventArgDescription("chatId", "Id of chat"),
    SignalRCustomEventDescription("Only owner can delete group. Both users can delete dm.")]
    public async Task DeleteChat(string chatId) {
        var user = _hubService.GetUserId(Context.ConnectionId);
        if (user == null) return;
        var err = await _db.DeleteChat(chatId, user);
        if (err != null) {
            await Clients.Caller.SendAsync("ErrorResponse", err);
        }
    }
    
    [InvokableSignalREvent("Used for adding user to group."),
    EventArgDescription("chatId", "Id of chat"),
    EventArgDescription("userId", "Id of user"),
    SignalRCustomEventDescription("Any user can invite to group. Users can't invite to dms.")]
    public async Task AddToGroup(string chatId, string userId) {
        var user = _hubService.GetUserId(Context.ConnectionId);
        if (user == null) return;
        var chat = await _db.AddToGroup(user, chatId, userId);
        if (chat != null) {
            await _hubService.SendMessage("ChatAdded", userId, JsonSerializer.Serialize(chat));
        }
    }

    [InvokableSignalREvent($"Used for notifying that client is typing in specific chat. Send every (" + RedisRepository.TYPING_LIFETIME_SECONDS + " - 1) seconds."),
    EventArgDescription("chatId", "Id of chat")]
    public async Task Typing(string chatId) {
        var user = _hubService.GetUserId(Context.ConnectionId);
        if (user == null) return;
        await _redisRepository.Typing(user, chatId);
    }

    [InvokableSignalREvent("Used for getting typing chats."),
    EventArgDescription("chatIds", "Ids of chats"),
    SignalRCustomEventDescription("Its very efficient if request multiple chats at once. You can invoke it very offen, like every one second. Suggested request time: (" + RedisRepository.TYPING_LIFETIME_SECONDS + " - 1) seconds."),
    EventWithResponse(
        "ChatsTyping",
        "Dictionary of typing chats",
        null,
        typeof(Dictionary<string, string>),
        "Dictionary chatId -> \"[userId]\" || (string)int; Dictionary with chatId keys and \"[userId]\" or int casted to string status; Int represents is a number of typing users. If \"[userId]\" - must be substitues with actual user name."
    )]
    public async Task GetChatsTyping(string[] chatIds) {
        var typing = await _redisRepository.GetTyping(chatIds);
        await Clients.Caller.SendAsync("ChatsTyping", typing);
    }

    [InvokableSignalREvent($"Used for editing message"),
    EventArgDescription("messageId", "Id of message"),
    EventArgDescription("newMessage", "New text of message"),
    SignalRCustomEventDescription("Message can be edited only by sender")]
    public async Task EditMessage(int messageId, string newMessage) {
        var user = _hubService.GetUserId(Context.ConnectionId);
        if (user == null) return;
        if (newMessage == null || newMessage.Length == 0) {
            await Clients.Caller.SendAsync("ErrorResponse", "Message is empty");
            return;
        }
        var err = await _db.EditMessage(messageId, newMessage, user);
        if (err != null) {
            await Clients.Caller.SendAsync("ErrorResponse", err);
        }
    }

    [InvokableSignalREvent($"Used for marking messages in specific chat as readen"),
    EventArgDescription("chatId", "Id of chat"),
    EventArgDescription("lastMessageId", "Id of last readen message"),
    SignalRCustomEventDescription("Send -1 as lastMessageId to mark all messages as readen")]
    public async Task ReadMessages(string chatId, int lastMessageId) {
        var user = _hubService.GetUserId(Context.ConnectionId);
        if (user == null) return;
        await _db.ReadMessages(chatId, user, lastMessageId);
    }

    [InvokableSignalREvent($"Used for deleting message"),
    EventArgDescription("messageId", "Id of message"),
    SignalRCustomEventDescription("Message can be deleted only by sender")]
    public async Task DeleteMessage(int messageId) {
        var user = _hubService.GetUserId(Context.ConnectionId);
        if (user == null) return;
        var err = await _db.DeleteMessage(messageId, user);
        if (err != null) {
            await Clients.Caller.SendAsync("ErrorResponse", err);
        }
    }

    [InvokableSignalREvent($"Used for renaming group"),
    EventArgDescription("chatId", "Id of chat"),
    EventArgDescription("name", "New name of group"),
    SignalRCustomEventDescription("Group can be renamed only by owner")]
    public async Task RenameGroup(string chatId, string name) {
        var user = _hubService.GetUserId(Context.ConnectionId);
        if (user == null) return;
        var err = await _db.RenameGroup(chatId, name, user);
        if (err != null) {
            await Clients.Caller.SendAsync("ErrorResponse", err);
        }
    }
}

