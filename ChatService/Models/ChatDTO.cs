using Cogni.Database.Entities;

namespace ChatService.Models;

public class ChatDto
{
    public Guid id { get; set; }
    public required string name { get; set; }
    public int ownerId { get; set; }
    public bool isDm { get; set; }
    public required List<int> members { get; set; }
    public LastMessageDto? lastMessage { get; set; }
    public int unreadCount { get; set; }

    public static ChatDto FromChatLast(Chat chat, LastMessageDto msg)
    {
        return new ChatDto
        {
            id = chat.Id,
            name = chat.Name,
            ownerId = chat.OwnerId,
            isDm = chat.isDm,
            members = chat.Members.Select(member => member.UserId).ToList(),
            lastMessage = msg,
            unreadCount = 1
        };
    }
}

public class LastMessageDto
{
    public int messageId { get; set; }
    public int senderId { get; set; }
    public string msg { get; set; } = string.Empty;
    public bool isFunctional { get; set; }
    public DateTime date { get; set; }

    public static LastMessageDto FromMessage(Message msg) {
        return new LastMessageDto
        {
            messageId = msg.MessageId,
            senderId = msg.SenderId,
            msg = msg.Msg,
            date = msg.Date,
            isFunctional = msg.IsFunctional
        };
    }
}