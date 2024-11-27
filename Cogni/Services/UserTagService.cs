using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Cogni.Database.Entities;
using Swashbuckle.Swagger;

namespace Cogni.Services
{
    public class UserTagService : IUserTagService
    {
        private readonly IUserTagRepository _usesTagRepository;

        public UserTagService(IUserTagRepository usesTagRepository)
        {
            this._usesTagRepository = usesTagRepository;
        }

        public async Task AddNewTagToUser(int userId, List<TagRequest> tag)
        {
            List<Database.Entities.Tag> tags = new List<Database.Entities.Tag>();
            foreach(var tagRequest in tag) 
            {
                tags.Add(new Database.Entities.Tag { Id =tagRequest.Id, NameTag= tagRequest.NameTag });
            }
            await _usesTagRepository.AddNewTagToUser(userId, tags);
        }

        public async Task<List<TagResponse>> GetUserTags(int userId)
        {
            var tags =await _usesTagRepository.GetUserTags(userId);
            List<TagResponse> newtags = new List<TagResponse>();
            foreach (var tagRequest in tags)
            {
                newtags.Add(new TagResponse (tagRequest.Id, tagRequest.NameTag ));
            }
            return newtags;
        }

        public async Task RemoveTagFromUser(int userId, List<TagRequest> tag)
        {
            List<Database.Entities.Tag> tags = new List<Database.Entities.Tag>();
            foreach (var tagRequest in tag)
            {
                tags.Add(new Database.Entities.Tag { Id = tagRequest.Id, NameTag = tagRequest.NameTag });
            }
            await _usesTagRepository.RemoveTagFromUser(userId, tags);
        }
    }
}
