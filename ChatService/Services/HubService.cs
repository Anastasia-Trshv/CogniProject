using System.Collections.Concurrent;
using System.Collections.Generic;
using ChatService.Abstractions;
using ChatService.Controllers;
using ChatService.Database.Context;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Services;

public class HubService : IHubService
{
    private readonly ILogger<HubService> _logger;

    private static readonly ConcurrentDictionary<string, string> ConnectionToUser = new();
    private static readonly ConcurrentDictionary<string, HashSet<string>> UserToConnections = new();

    private readonly IHubContext<ChatHubController> _hubContext;

    public HubService(IHubContext<ChatHubController> hubContext, ILogger<HubService> logger)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task SendMessage(string method, string userId, string message)
    {
        if (!UserToConnections.ContainsKey(userId))
        {
            return;
        }

        foreach (var connectionId in UserToConnections[userId])
        {
            _logger.LogWarning($"[{method}] Sending message to connection: {connectionId}");
            await _hubContext.Clients.Client(connectionId).SendAsync(method, message);
        }
    }

    public void AddRel(string userId, string connectionId)
    {
        if (!string.IsNullOrEmpty(userId)){
            ConnectionToUser[connectionId] = userId;
            if (!UserToConnections.ContainsKey(userId))
                UserToConnections[userId] = new HashSet<string>();
            UserToConnections[userId].Add(connectionId);
        }
    }

    public void RemoveRel(string connectionId){
        if (ConnectionToUser.TryRemove(connectionId, out var userId))
        {
            if (UserToConnections.ContainsKey(userId))
            {
                UserToConnections[userId].Remove(connectionId);
                if (UserToConnections[userId].Count == 0)
                    UserToConnections.Remove(userId, out _);
            }
        }
    }
    
    public string? GetUserId(string connectionId) => ConnectionToUser.TryGetValue(connectionId, out var userId) ? userId : null;
}