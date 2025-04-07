using ChatService.Abstractions;
using StackExchange.Redis;

namespace ChatService.Repository;

public class RedisRepository : IRedisRepository {
    public const string ONLINE_LIFETIME_SECONDS = "3";
    public const string TYPING_LIFETIME_SECONDS = "3";
    private readonly IDatabase _redisDb;
    private readonly ILogger<RedisRepository> _logger;

    public RedisRepository(
        IConnectionMultiplexer redis,
        ILogger<RedisRepository> logger
    ){
        _redisDb = redis.GetDatabase();
        _logger = logger;
    }

    public async Task UserHere(string userId) {
        await _redisDb.StringSetAsync($"ONLINE:{userId}", userId.ToString(), TimeSpan.FromSeconds(int.Parse(ONLINE_LIFETIME_SECONDS)));
    }

    public async Task<Dictionary<string, bool>> GetOnline(string[] userIds) {
        var status = new Dictionary<string, bool>();
        var toGet = new List<RedisKey>();
        foreach (var userId in userIds) {
            toGet.Add($"ONLINE:{userId}");
        }
        var onlineStatus = await _redisDb.StringGetAsync(toGet.ToArray());
        for (int i = 0; i < userIds.Length; i++) {
            status[userIds[i]] = !onlineStatus[i].IsNullOrEmpty;
        }
        return status;
    }

    public async Task Typing(string userId, string chatId)
    {
        var expirationTime = DateTimeOffset.UtcNow.AddSeconds(int.Parse(TYPING_LIFETIME_SECONDS)).ToUnixTimeSeconds();
        await _redisDb.SortedSetAddAsync($"TYPING:{chatId}", $"[{userId}]", expirationTime);
    }

    public async Task StopTyping(string userId, string chatId)
    {
        await _redisDb.SortedSetRemoveAsync($"TYPING:{chatId}", $"[{userId}]");
    }

    public async Task<Dictionary<string, string?>> GetTyping(string[] chatIds)
    {
        var typing = new Dictionary<string, string?>();
        foreach (var chatId in chatIds){
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            await _redisDb.SortedSetRemoveRangeByScoreAsync($"TYPING:{chatId}", double.NegativeInfinity, currentTime);
            var members = await _redisDb.SortedSetRangeByScoreAsync($"TYPING:{chatId}", double.NegativeInfinity, double.PositiveInfinity);
            var t = members.Select(m => m.ToString()).ToList();
            if (t.Count == 1) {
                typing[chatId] = t[0];
            } else if (t.Count > 1) {
                typing[chatId] = t.Count.ToString();
            }
        }
        return typing;
    }
}