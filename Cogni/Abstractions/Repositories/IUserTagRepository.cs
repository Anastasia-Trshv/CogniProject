using Cogni.Database.Entities;

namespace Cogni.Abstractions.Repositories
{
    public interface IUserTagRepository
    {
        Task<List<Tag>> GetUserTags(int userId);
        Task<List<Hobby>> GetHobbies();
        Task<List<int>> GetUserTagCategoryIds(int userId);
        Task<List<int>> GetUserTagIds(int userId);
        Task SetUserTagSelection(int userId, List<int> categoryIds, List<int> tagIds);
        Task AddNewTagToUser(int userId, List<Tag> tag);
        Task RemoveTagFromUser(int userId, List<Tag> tag);

    }
}
