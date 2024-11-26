using Cogni.Abstractions.Repositories;

namespace Cogni.Database.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        public Task<List<Friend>> GetAllUserFriends(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<(int id, string picUrl)>> GetFriendsPreview(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNumOfFriends(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
