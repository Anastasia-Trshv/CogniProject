using Cogni.Abstractions.Repositories;
using Cogni.Database.Context;
using Cogni.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cogni.Database.Repositories
{
    public class UserTagRepository : IUserTagRepository
    {
        private readonly CogniDbContext _cogniDbContext;
        public UserTagRepository(CogniDbContext cogniDbContext)
        {
            _cogniDbContext = cogniDbContext;
        }
        public async Task AddNewTagToUser(int userId, List<Tag> tag)
        {
            foreach (var tagItem in tag)
            {
                await _cogniDbContext.UserTags.AddAsync(new UserTag { IdTag = tagItem.Id, IdUser = userId, IdTagNavigation = tagItem});
            }
        }

        public async Task<List<Tag>> GetUserTags(int userId)
        {
            return await _cogniDbContext.UserTags
        .Where(u => u.IdUser == userId)
        .Include(ut => ut.IdTagNavigation)
        .Select(ut => ut.IdTagNavigation)
        .ToListAsync();
        }

        public async Task RemoveTagFromUser(int userId, List<Tag> tag)
        {
            foreach (var tagItem in tag)
            {
                var userTag = await _cogniDbContext.UserTags.Where(u => u.IdUser == userId && u.IdTagNavigation == tagItem).FirstOrDefaultAsync();
                if (userTag != null)
                {
                    _cogniDbContext.UserTags.Remove(userTag);
                }
            }
            await _cogniDbContext.SaveChangesAsync();
        }
    
    }
}
