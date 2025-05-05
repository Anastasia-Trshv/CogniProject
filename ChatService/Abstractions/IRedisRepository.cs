namespace ChatService.Abstractions;

public interface IRedisRepository
{
    Task UserHere(string key);
    Task<Dictionary<string, bool>> GetOnline(string[] keys);
    Task Typing(string user, string chatId);
    Task StopTyping(string user, string chatId);
    Task<Dictionary<string, string?>> GetTyping(string[] keys);
}