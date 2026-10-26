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
            var requestedTagIds = tag.Select(item => item.Id).Distinct().ToList();
            var existingTagIds = await _cogniDbContext.UserTags
                .Where(item => item.IdUser == userId && requestedTagIds.Contains(item.IdTag))
                .Select(item => item.IdTag)
                .ToListAsync();
            var newTags = requestedTagIds.Except(existingTagIds)
                .Select(id => new UserTag { IdTag = id, IdUser = userId });
            await _cogniDbContext.UserTags.AddRangeAsync(newTags);
            await _cogniDbContext.SaveChangesAsync();
        }

        public async Task<List<Tag>> GetUserTags(int userId)
        {
            return await _cogniDbContext.UserTags
        .Where(u => u.IdUser == userId)
        .Include(ut => ut.IdTagNavigation)
        .Select(ut => ut.IdTagNavigation)
        .ToListAsync();
        }

        public async Task<List<Hobby>> GetHobbies()
        {
            return await _cogniDbContext.Hobbies
                .Include(hobby => hobby.Categories)
                .ThenInclude(category => category.Tags)
                .OrderBy(hobby => hobby.Name)
                .ToListAsync();
        }

        public async Task<List<int>> GetUserTagCategoryIds(int userId)
        {
            return await _cogniDbContext.UserTagCategories
                .Where(item => item.IdUser == userId)
                .Select(item => item.IdCategory)
                .ToListAsync();
        }

        public async Task<List<int>> GetUserTagIds(int userId)
        {
            return await _cogniDbContext.UserTags
                .Where(item => item.IdUser == userId)
                .Where(item => item.IdTagNavigation.IdCategory != null)
                .Select(item => item.IdTag)
                .OrderBy(id => id)
                .ToListAsync();
        }

        public async Task SetUserTagSelection(int userId, List<int> categoryIds, List<int> tagIds)
        {
            await using var transaction = await _cogniDbContext.Database.BeginTransactionAsync();

            var oldCategories = await _cogniDbContext.UserTagCategories
                .Where(item => item.IdUser == userId)
                .ToListAsync();
            var oldTags = await _cogniDbContext.UserTags
                .Where(item => item.IdUser == userId)
                .ToListAsync();

            _cogniDbContext.UserTagCategories.RemoveRange(oldCategories);
            _cogniDbContext.UserTags.RemoveRange(oldTags);
            await _cogniDbContext.SaveChangesAsync();

            _cogniDbContext.UserTagCategories.AddRange(categoryIds.Distinct().Select(id =>
                new UserTagCategory
                {
                    IdUser = userId,
                    IdCategory = id
                }));
            _cogniDbContext.UserTags.AddRange(tagIds.Distinct().Select(id =>
                new UserTag { IdUser = userId, IdTag = id }));

            await _cogniDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task RemoveTagFromUser(int userId, List<Tag> tag)
        {
            foreach (var tagItem in tag)
            {
                var userTag = await _cogniDbContext.UserTags.Where(u => u.IdUser == userId && u.IdTag == tagItem.Id).FirstOrDefaultAsync();
                if (userTag != null)
                {
                    _cogniDbContext.UserTags.Remove(userTag);
                }
            }
            await _cogniDbContext.SaveChangesAsync();
        }
    
    }
}
