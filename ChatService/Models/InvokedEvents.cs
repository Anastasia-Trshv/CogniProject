using System.Text;
using System.Text.Json;
using ChatService.Abstractions;
using ChatService.CustomSwaggerGen;
using ChatService.Database.Context;
using ChatService.Database.Entities;
using ChatService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatService.Models;

// это вам не rust ☠️, никаких трейтов
// also, [JsonProperty("lastName")] attribute exists
public abstract class InvokedEvent
{
    public const int MAX_ATTACHMENTS = 9;
    public virtual string type { get; } = "";
    public virtual async Task Update(IChatRepository _db){}
    public virtual async Task<List<string>> GetRecievers(IChatRepository _db){ return new (); }

    public static InvokedEvent? DeserializeEvent(string json) {
        try {
            var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);
            var eventType = jsonObject.GetProperty("type").GetString();
            return eventType switch
            {
                "NewChatAdded" => JsonSerializer.Deserialize<ChatAddedEvent>(json),
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


[ListenableSignalREvent("Happens when new chat is created")]
public class ChatAddedEvent : InvokedEvent {
    [CustomEventDescription("Chat id")]
    public required string chatId { get; set; }
    [CustomEventDescription("User name")]
    public required string name { get; set; }
    [CustomEventDescription("Is it direct between two users")]
    public bool isDm { get; set; }
    [CustomEventDescription("Chat members userIds")]
    public required List<string> members { get; set; }
    [ListenableEventName]
    public override string type => "NewChatAdded";
    [CustomEventDescription("Last message object")]
    public LastMessageDto? lastMessage { get; set; }
    [CustomEventDescription("Number of unread messages")]
    public int unreadCount { get; set; }
    public override async Task Update(IChatRepository _db) => this.members = await _db.GetChatMembers(this.chatId);
    public override async Task<List<string>> GetRecievers(IChatRepository _db) => this.members;
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));

    public static ChatAddedEvent FromNewChat(Chat chat, LastMessageDto lastMessage) {
        return new ChatAddedEvent
        {
            chatId = chat.Id.ToString(),
            name = chat.Name,
            members = chat.Members.Select(m => m.UserId.ToString()).ToList(),
            isDm = chat.isDm,
            lastMessage = lastMessage,
            unreadCount = 1 // нуаче)))
        };
    }
}


[ListenableSignalREvent("Happens when new message is sent")]
public class NewMessageEvent : InvokedEvent {
    [CustomEventDescription("Message id; Message id is ordered and isn't chat-unique")]
    public int messageId { get; set; }
    [CustomEventDescription("Chat id")]
    public required string chatId { get; set; }
    [CustomEventDescription("UserId of sender")]
    public required string senderId { get; set; }
    [CustomEventDescription("Text of message")]
    public required string msg { get; set; }
    [CustomEventDescription("Date of sending")]
    public DateTime date { get; set; }
    [CustomEventDescription("Is that message was edited")]
    public bool isEdited { get; set; } = false;
    [CustomEventDescription("Is that message functional; Example: \"[userId] invited [userId] to group.\"; Garanteed, that only userIds are surrounded by brackets []; You must substitue them with user names due usernames can be changed")]
    public bool isFunctional { get; set; } = false;
    [CustomEventDescription("List of links to attachments; File extension is always lowercase; File extenstion isn't garanteed. Link is app-relative; Example link: \"/chat-files/e4009e0a-a314-4193-a4e1-2d8efcef4a9f_cat.png\"; My idea for sending files is: If it starts with \"FILE::\" - its a file, other logic must be implemented on fronend;")]
    public required List<string> attachments { get; set; }
    [ListenableEventName]
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

[ListenableSignalREvent("Happens when chat is removed")]
public class ChatRemovedEvent : InvokedEvent {
    [CustomEventDescription("Chat id")]
    public required string id { get; set; }
    [ListenableEventName]
    public override string type => "ChatRemoved";
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
    public required List<string> to_send { get; set; }
    public override async Task<List<string>> GetRecievers(IChatRepository _db) => this.to_send;
    public static ChatRemovedEvent FromChatId(string id, List<string> to_send) => new ChatRemovedEvent { id = id, to_send = to_send };
}

[ListenableSignalREvent("Happens when same account reads messages")]
public class MessagesReadenEvent : InvokedEvent {
    [CustomEventDescription("Chat id")]
    public required string chatId { get; set; }
    [CustomEventDescription("Id of last message; Message id is ordered and isn't chat-unique")]
    public int lastMessageId { get; set; }
    [CustomEventDescription("Number of unread messages")]
    public int unreadCount { get; set; }
    public required string user_id { get; set; } // only for delivery
    [ListenableEventName]
    public override string type => "MsgsReaden";
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
    public override async Task<List<string>> GetRecievers(IChatRepository _db) => [this.user_id];

}

[ListenableSignalREvent("Happens when message is deleted")]
public class MessageDeletedEvent : InvokedEvent {
    [CustomEventDescription("Id of deleted message; Message id is ordered and isn't chat-unique")]
    public int messageId { get; set; }
    [CustomEventDescription("UserId of sender")]
    public string senderId { get; set; }
    [CustomEventDescription("Chat id")]
    public required string chatId { get; set; }
    [ListenableEventName]
    public override string type => "MsgDeleted";
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
    public override async Task<List<string>> GetRecievers(IChatRepository _db) => await _db.GetChatMembers(this.chatId);
}

[ListenableSignalREvent("Happens when message is edited")]
public class MessageEditedEvent : InvokedEvent {
    [CustomEventDescription("Chat id")]
    public required string chatId { get; set; }
    [CustomEventDescription("Id of edited message; Message id is ordered and isn't chat-unique")]
    public int messageId { get; set; }
    [CustomEventDescription("New message text")]
    public required string newMessage { get; set; }
    [CustomEventDescription("UserId of sender")]
    public string senderId { get; set; }
    [ListenableEventName]
    public override string type => "MsgEdited";
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
    public override async Task<List<string>> GetRecievers(IChatRepository _db) => await _db.GetChatMembers(this.chatId);
}

[ListenableSignalREvent("Happens when group is renamed")]
public class GroupRenamedEvent : InvokedEvent {
    [CustomEventDescription("Chat id")]
    public required string chatId { get; set; }
    [CustomEventDescription("New group name")]
    public required string newName { get; set; }
    [ListenableEventName]
    public override string type => "GroupRenamed";
    public override byte[] Serialize() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
    public override async Task<List<string>> GetRecievers(IChatRepository _db) => await _db.GetChatMembers(this.chatId);
}

