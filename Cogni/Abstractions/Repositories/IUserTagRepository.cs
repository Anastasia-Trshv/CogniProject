using Cogni.Database.Entities;

namespace Cogni.Abstractions.Repositories
{
    public interface IUserTagRepository
    {
        Task<List<Tag>> GetUserTags(int userId);
        Task AddNewTagToUser(int userId, List<Tag> tag);
        Task RemoveTagFromUser(int userId, List<Tag> tag);

    }
}
