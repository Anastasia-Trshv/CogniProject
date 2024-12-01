using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;

namespace Cogni.Services
{
    public class FriendService : IFriendService
    {
        private readonly IFriendRepository _friendRepository;
        public FriendService(IFriendRepository friendRepository)
        {
            this._friendRepository = friendRepository;
        }

        public async Task<List<Friend>> GetAllUserFriends(int userId)
        {
           var list = await _friendRepository.GetAllUserFriends(userId);
            return list;
        }

        public async Task<List<(int id, string picUrl)>> GetFriendsPreview(int userId)
        {
           var list = await _friendRepository.GetFriendsPreview(userId);
            return list;
        }

        public async Task<int> GetNumOfFriends(int userId)
        {
            return await _friendRepository.GetNumOfFriends(userId);
        }
    }
}
