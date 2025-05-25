using Cogni.Database.Entities;
using ChatService.Models;

namespace ChatService.Abstractions;

public interface IChatRepository
{
    Task<string?> SendEvent(byte[] body);
    Task<List<ChatDto>> GetChatList(string userId);
    Task<string?> CreateGroup(string userId, string groupName, List<string> users);
    Task<List<Message>> GetMsgs(string chatId, string userId, int startId, int amount, bool toNew);
    Task<Message?> SendMsg(string chatId, string userId, string msg, List<string> attachments);
    Task<List<string>> GetChatMembers(string chatId);
    Task<Chat?> StartDm(string initUserId, string userId, string message);
    Task<string?> LeaveGroup(string chatId, string userId);
    Task<string?> DeleteChat(string chatId, string userId);
    Task AddToGroup(string invokerId, string chatId, string userId);
    Task<string?> EditMessage(int messageId, string new_message, string userId);
    Task ReadMessages(string chatId, string userId, int lastMessageId);
    Task<string?> DeleteMessage(int messageId, string userId);
    Task<string?> RenameGroup(string chatId, string new_name, string userId);
}