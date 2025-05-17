using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Models;


namespace Cogni.Services;

public class FriendService : IFriendService
{
    private readonly IFriendRepository _friendRepository;
    public FriendService(IFriendRepository friendRepository)
    {
        _friendRepository = friendRepository;
    }

    public async Task<List<FriendDto>> GetUserFriends(int userId)
    {
        var list = await _friendRepository.GetUserFriends(userId);
        return list;
    }

    public async Task<List<(int id, string? picUrl)>> GetFriendsPreview(int userId)
    {
        var list = await _friendRepository.GetFriendsPreview(userId);
        return list;
    }

    public async Task<int> GetNumOfFriends(int userId)
    {
        return await _friendRepository.GetNumOfFriends(userId);
    }

    public async Task Subscribe(int userId, int friendId)
    {
        await _friendRepository.Subscribe(userId, friendId);
    }

    public async Task Unsubscribe(int userId, int friendId)
    {
        await _friendRepository.Unsubscribe(userId, friendId);
    }

    public async Task<SubscribeDTO> CheckSubscribe(int userId, int friendId)
    {
        return await _friendRepository.CheckSubscribe(userId, friendId);
    }
}
