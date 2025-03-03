using Cogni.Abstractions.Repositories;
using Cogni.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Cogni.Database.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        // todo: put it somewhere else
        private readonly string placeholderImage = "https://yt3.googleusercontent.com/cBNnmASNGeUEDG3ij8pHF6592DTJWnwRPsrAGIql7p5P7hdw9VwQ_HJdZG9Pwjk806tQCMTbhw=s900-c-k-c0x00ffffff-no-rj";
        private readonly CogniDbContext _context;
        public FriendRepository(CogniDbContext context)
        {
            _context = context;
        }
        public async Task<List<FriendDto>> GetUserFriends(int userId)
        {
            var friends = await _context.Users
                .Where(u => _context.Friends.Any(f => f.UserId == userId && f.FriendId == u.Id) && 
                            _context.Friends.Any(f => f.UserId == u.Id && f.FriendId == userId))
                .Select(u => new FriendDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    PicUrl = u.Avatars.Where(a => a.IsActive ?? false).Select(a => a.AvatarUrl).FirstOrDefault(),
                    Mbti = u.IdMbtiTypeNavigation.Id
                })
                .ToListAsync();
            return friends;
        }

        public async Task Subscribe(int userId, int friendId)
        {
            var friend = new Friend
            {
                UserId = userId,
                FriendId = friendId
            };
            await _context.Friends.AddAsync(friend);
            await _context.SaveChangesAsync();
        }

        public async Task Unsubscribe(int userId, int friendId)
        {
            var friend = await _context.Friends
                .FirstOrDefaultAsync(f => f.UserId == userId && f.FriendId == friendId);
            if (friend == null)
            {
                // todo: maybe we should throw an exception here
                return;
            }
            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
        }

        public async Task<List<(int id, string? picUrl)>> GetFriendsPreview(int userId)
        {
            var friends = await _context.Users
            .Where(u => _context.Friends.Any(f => f.UserId == userId && f.FriendId == u.Id) && 
                        _context.Friends.Any(f => f.UserId == u.Id && f.FriendId == userId))
            .Select(u => new
            {
                id = u.Id,
                picUrl = u.Avatars.Where(a => a.IsActive ?? false).Select(a => a.AvatarUrl).FirstOrDefault()
            })
            .Take(6)
            .ToListAsync();

            return friends.Select(f => (f.id, f.picUrl)).ToList();
        }

        public async Task<int> GetNumOfFriends(int userId)
        {
           var list = await _context.Friends
                .Where(f1 => f1.UserId == userId)
                .Where(f1 => _context.Friends.Any(f2 => f2.UserId == f1.FriendId && f2.FriendId == userId)).CountAsync();
            return list;
        }
    }
}
