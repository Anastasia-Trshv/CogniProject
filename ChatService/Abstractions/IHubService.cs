namespace ChatService.Abstractions;

public interface IHubService
{
    public Task SendMessage(string method, string userId, string message);
    public void AddRel(string userId, string connectionId);
    public void RemoveRel(string connectionId);
    public string? GetUserId(string connectionId);
}