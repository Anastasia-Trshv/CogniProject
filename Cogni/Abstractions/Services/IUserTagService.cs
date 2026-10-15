using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Cogni.Database.Entities;

namespace Cogni.Abstractions.Services
{
    public interface IUserTagService
    {
        Task<List<TagResponse>> GetUserTags(int userId);
        Task<List<HobbyResponse>> GetHobbyCatalog();
        Task<UserTagSelectionResponse> GetUserTagSelection(int userId);
        Task SetUserTagSelection(int userId, SetUserTagSelectionRequest selection);
        Task AddNewTagToUser(int userId, List<AddTagToUserRequest> tag);
        Task RemoveTagFromUser(int userId, List<AddTagToUserRequest> tag);
    }
}
