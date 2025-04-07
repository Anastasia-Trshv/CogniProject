namespace ChatService.Abstractions;

public interface IHubService
{
    public Task SendMessage<T>(string method, string userId, T message);
    public void AddRel(string userId, string connectionId);
    public void RemoveRel(string connectionId);
    public string? GetUserId(string connectionId);
}