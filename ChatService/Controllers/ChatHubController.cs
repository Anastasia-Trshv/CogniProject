using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using ChatService.Abstractions;
using ChatService.Database.Context;
using ChatService.Entities;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Controllers {
    public class ChatHubController : Hub
    {
        private static readonly ConcurrentDictionary<string, string> ConnectionToUser = new();
        private static readonly ConcurrentDictionary<string, HashSet<string>> UserToConnections = new();
        private readonly ILogger<ChatHubController> _logger;
        private readonly IHubService _hubService;
        private readonly IChatRepository _db;
        private readonly IRedisRepository _redisRepository;
        public ChatHubController(
            ILogger<ChatHubController> logger,
            IHubService hubService,
            IRedisRepository redis,
            IChatRepository db
        ) {
            _logger = logger;
            _hubService = hubService;
            _redisRepository = redis;
            _db = db;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogWarning($"Connected: {Context.ConnectionId}");
            var userId = Context.GetHttpContext()?.Request.Query["userId"];
            // TODO: AUTHORITY CHECK
            _hubService.AddRel(userId, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogWarning($"Disconnected: {Context.ConnectionId}");
            _hubService.RemoveRel(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ImHere()
        {
            var user = _hubService.GetUserId(Context.ConnectionId);
            _logger.LogDebug($"ImHere: {user}");
            if (user == null) return;
            await _redisRepository.UserHere(user);
        }

        public async Task GetUsersOnline(string[] userIds)
        {
            _logger.LogDebug($"GetUsersOnline: {userIds}");
            var online = await _redisRepository.GetOnline(userIds);
            if (online.Count == 0) return;
            await Clients.Caller.SendAsync("UsersOnline", online);
        }

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


        public async Task CreateGroup(string groupName, List<string> users) {
            var user = _hubService.GetUserId(Context.ConnectionId);
            if (user == null) return;
            var err = await _db.CreateGroup(user, groupName, users);
            if (err != null) {
                await Clients.Caller.SendAsync("ErrorResponse", err);
            }
        }

        public async Task GetMsgs(string chatId, int startId, bool toNew, int amount) {
            _logger.LogWarning($"Getting msgs for chatId: {chatId} {startId} {amount}");
            var user = _hubService.GetUserId(Context.ConnectionId);
            if (user == null) return;
            var msgs = await _db.GetMsgs(chatId, user, startId, amount, toNew);
            await Clients.Caller.SendAsync("Msgs", msgs);
        }

        public async Task SendMsg(string chatId, string msg, List<string> attachments) {
            var user = _hubService.GetUserId(Context.ConnectionId);
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

        public async Task StartDm(string userId, string message) {
            var user = _hubService.GetUserId(Context.ConnectionId);
            if (user == null) return;
            var chat = await _db.StartDm(user, userId, message);
            if (chat == null){
                await Clients.Caller.SendAsync("ErrorResponse", $"Cant start a dm with {userId}!");
            }
        }

        public async Task LeaveGroup(string chatId) {
            var user = _hubService.GetUserId(Context.ConnectionId);
            if (user == null) return;
            var err = await _db.LeaveGroup(chatId, user);
            if (err != null) {
                await Clients.Caller.SendAsync("ErrorResponse", err);
            }
        }
        public async Task DeleteChat(string chatId) {
            var user = _hubService.GetUserId(Context.ConnectionId);
            if (user == null) return;
            var err = await _db.DeleteChat(chatId, user);
            if (err != null) {
                await Clients.Caller.SendAsync("ErrorResponse", err);
            }
        }

        public async Task AddToGroup(string chatId, string userId) {
            var user = _hubService.GetUserId(Context.ConnectionId);
            if (user == null) return;
            var chat = await _db.AddToGroup(user, chatId, userId);
            if (chat != null) {
                await _hubService.SendMessage("ChatAdded", userId, JsonSerializer.Serialize(chat));
            }
        }

        public async Task Typing(string chatId) {
            var user = _hubService.GetUserId(Context.ConnectionId);
            if (user == null) return;
            await _redisRepository.Typing(user, chatId);
        }

        public async Task GetChatsTyping(string[] chatIds) {
            var typing = await _redisRepository.GetTyping(chatIds);
            await Clients.Caller.SendAsync("ChatsTyping", typing);
        }

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

        public async Task ReadMessages(string chatId, int lastMessageId) {
            var user = _hubService.GetUserId(Context.ConnectionId);
            if (user == null) return;
            await _db.ReadMessages(chatId, user, lastMessageId);
        }

        public async Task DeleteMessage(int messageId) {
            var user = _hubService.GetUserId(Context.ConnectionId);
            if (user == null) return;
            var err = await _db.DeleteMessage(messageId, user);
            if (err != null) {
                await Clients.Caller.SendAsync("ErrorResponse", err);
            }
        }

        public async Task RenameGroup(string chatId, string name) {
            var user = _hubService.GetUserId(Context.ConnectionId);
            if (user == null) return;
            var err = await _db.RenameGroup(chatId, name, user);
            if (err != null) {
                await Clients.Caller.SendAsync("ErrorResponse", err);
            }
        }
    }
}
