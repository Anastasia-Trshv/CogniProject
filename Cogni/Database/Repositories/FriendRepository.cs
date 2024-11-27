using Cogni.Abstractions.Repositories;
using Cogni.Database.Context;
using Cogni.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cogni.Database.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly CogniDbContext _context;
        public FriendRepository(CogniDbContext context)
        {
            _context = context;
        }
        public async Task<List<Friend>> GetAllUserFriends(int userId)
        {
           var list = await _context.Friends.Where(u => u.UserId == userId || u.FriendId == userId).ToListAsync();
            return list;
        }

        public async Task<List<(int id, string picUrl)>> GetFriendsPreview(int userId)
        {
            var result = await _context.Friends
            .Where(f => f.UserId == userId || f.FriendId == userId)
            .Take(6)
            .ToListAsync();
            List<int> list = new List<int>();
            foreach (var f in result)
            {
                if (f.UserId == userId)
                {
                    list.Add(f.FriendId);
                }
                else { list.Add(f.UserId); }
            }
            List<(int, string)> list2 = new List<(int, string)> ();
            List<User> users = new List<User> ();
            foreach (var f in list)
            {
                var e = await _context.Users
                     .Include(u => u.Avatars)
                    .FirstOrDefaultAsync(u => u.Id == userId);
                users.Add(e);
            }
            foreach (var f in users)
            {
                var e = f.Avatars.FirstOrDefault(a => a.IsActive ==true);
                list2.Add((f.Id, e.AvatarUrl));
            }
            
            return list2;
        }

        public async Task<int> GetNumOfFriends(int userId)
        {
            return await _context.Friends.Where(f => f.UserId == userId || f.FriendId == userId).CountAsync();
        }
    }
}
