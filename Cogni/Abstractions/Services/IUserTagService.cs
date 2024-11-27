using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Cogni.Database.Entities;

namespace Cogni.Abstractions.Services
{
    public interface IUserTagService
    {
        Task<List<TagResponse>> GetUserTags(int userId);
        Task AddNewTagToUser(int userId, List<TagRequest> tag);
        Task RemoveTagFromUser(int userId, List<TagRequest> tag);
    }
}
