using System.Text;
using System.Text.Json;
using ChatService.Abstractions;
using ChatService.Database.Context;
using ChatService.Database.Entities;
using ChatService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatService.Models;

// это вам не rust ☠️, никаких трейтов

public abstract class InvokedEvent
{
    public const int MAX_ATTACHMENTS = 9;
    public virtual string type { get; } = "";


    public virtual async Task<string?> Verify(IChatRepository _db){
        if ((await this.GetRecievers(_db)).Count == 0) return "No recievers, nothing to do!";
        return null;
    }
    public virtual async Task<List<string>> GetRecievers(IChatRepository _db){ return new (); }

    public static InvokedEvent? DeserializeEvent(string json) {
        try {
            var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);
            var eventType = jsonObject.GetProperty("type").GetString();
            return eventType switch
            {
                "ChatAdded" => JsonSerializer.Deserialize<ChatAddedEvent>(json),
                "NewMsg" => JsonSerializer.Deserialize<NewMessageEvent>(json),
                "ChatRemoved" => JsonSerializer.Deserialize<ChatRemovedEvent>(json),
                "MsgsReaden" => JsonSerializer.Deserialize<MessagesReadenEvent>(json),
                "MsgDeleted" => JsonSerializer.Deserialize<MessageDeletedEvent>(json),
                "MsgEdited" => JsonSerializer.Deserialize<MessageEditedEvent>(json),
                "GroupRenamed" => JsonSerializer.Deserialize<GroupRenamedEvent>(json),
                _ => null
            };
        } catch (Exception _ex) {
            return null;
        }
    }

    public virtual byte[] Serialize(){
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
    }
}

public class ChatAddedEvent : InvokedEvent {
    public required string id { get; set; }
    public required string name { get; set; }
    public bool isDm { get; set; }
    public required List<string> members { get; set; }
    public override string type => "ChatAdded";
    public LastMessageDto? lastMessage { get; set; }
    public int unreadCount { get; set; }
    public override async Task<List<string>> GetRecievers(IChatRepository _db) => await _db.GetChatMembers(this.id);
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));

    public static ChatAddedEvent FromNewChat(Chat chat, LastMessageDto lastMessage) {
        return new ChatAddedEvent
        {
            id = chat.Id.ToString(),
            name = chat.Name,
            members = chat.Members.Select(m => m.UserId.ToString()).ToList(),
            isDm = chat.isDm,
            lastMessage = lastMessage,
            unreadCount = 1 // нуаче)))
        };
    }
}


public class NewMessageEvent : InvokedEvent {
    public int messageId { get; set; }
    public required string chatId { get; set; }
    public required string senderId { get; set; }
    public required string msg { get; set; }
    public DateTime date { get; set; }
    public bool isEdited { get; set; } = false;
    public bool isFunctional { get; set; } = false;
    public required List<string> attachments { get; set; }
    public override string type => "NewMsg";
    public override async Task<List<string>> GetRecievers(IChatRepository _db){
        // since sent, new users can be added.
        return await _db.GetChatMembers(this.chatId);
    }
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));

    public static NewMessageEvent FromMessage(Message msg) {
        return new NewMessageEvent
        {
            messageId = msg.MessageId,
            chatId = msg.ChatId.ToString(),
            senderId = msg.SenderId.ToString(),
            msg = msg.Msg,
            date = msg.Date,
            isEdited = msg.IsEdited,
            isFunctional = msg.IsFunctional,
            attachments = msg.Attachments
        };
    }
}

public class ChatRemovedEvent : InvokedEvent {
    public required string id { get; set; }
    public override string type => "ChatRemoved";
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
    public required List<string> to_send { get; set; }
    public override async Task<List<string>> GetRecievers(IChatRepository _db) => this.to_send;
    public static ChatRemovedEvent FromChatId(string id, List<string> to_send) => new ChatRemovedEvent { id = id, to_send = to_send };
}

public class MessagesReadenEvent : InvokedEvent {
    public required string chatId { get; set; }
    public int lastMessageId { get; set; }
    public int unreadCount { get; set; }
    public required string userId { get; set; } // only for delivery
    public override string type => "MsgsReaden";
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
    public override async Task<List<string>> GetRecievers(IChatRepository _db) => [this.userId];

}

public class MessageDeletedEvent : InvokedEvent {
    public int messageId { get; set; }
    public string senderId { get; set; }
    public required string chatId { get; set; }
    public override string type => "MsgDeleted";
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
    public override async Task<List<string>> GetRecievers(IChatRepository _db) => await _db.GetChatMembers(this.chatId);
}

public class MessageEditedEvent : InvokedEvent {
    public required string chatId { get; set; }
    public int messageId { get; set; }
    public required string newMessage { get; set; }
    public string senderId { get; set; }
    public override string type => "MsgEdited";
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
    public override async Task<List<string>> GetRecievers(IChatRepository _db) => await _db.GetChatMembers(this.chatId);
}

public class GroupRenamedEvent : InvokedEvent {
    public required string chatId { get; set; }
    public required string newName { get; set; }
    public override string type => "GroupRenamed";
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
    public override async Task<List<string>> GetRecievers(IChatRepository _db) => await _db.GetChatMembers(this.chatId);
}