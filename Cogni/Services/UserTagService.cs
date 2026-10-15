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
        private static readonly HashSet<string> FixedHobbyNames = new(StringComparer.Ordinal)
        {
            "Музыка",
            "Медиа",
            "Творчество",
            "Игры",
            "Спорт"
        };

        private readonly IUserTagRepository _usesTagRepository;

        public UserTagService(IUserTagRepository usesTagRepository)
        {
            this._usesTagRepository = usesTagRepository;
        }

        public async Task AddNewTagToUser(int userId, List<AddTagToUserRequest> tag)
        {
            List<Database.Entities.Tag> tags = new List<Database.Entities.Tag>();
            foreach(var tagRequest in tag) 
            {
                tags.Add(new Database.Entities.Tag { Id =tagRequest.Id});
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

        public async Task<List<HobbyResponse>> GetHobbyCatalog()
        {
            var hobbies = await _usesTagRepository.GetHobbies();
            return hobbies
                .Where(hobby => FixedHobbyNames.Contains(hobby.Name))
                .Select(hobby => new HobbyResponse(
                hobby.Id,
                hobby.Name,
                hobby.Categories
                    .OrderBy(category => category.Name)
                    .Select(category => new TagCategoryResponse(
                        category.Id,
                        category.Name,
                        category.Tags
                            .Where(tag => tag.NameTag != null)
                            .OrderBy(tag => tag.NameTag)
                            .Select(tag => new TagResponse(tag.Id, tag.NameTag))
                            .ToList()))
                    .ToList()))
                .ToList();
        }

        public async Task<UserTagSelectionResponse> GetUserTagSelection(int userId)
        {
            var categoryIds = await _usesTagRepository.GetUserTagCategoryIds(userId);
            var tagIds = await _usesTagRepository.GetUserTagIds(userId);
            return new UserTagSelectionResponse(categoryIds, tagIds);
        }

        public async Task SetUserTagSelection(int userId, SetUserTagSelectionRequest selection)
        {
            if (selection.CategoryIds is null || selection.TagIds is null)
                throw new ArgumentException("CategoryIds and TagIds are required.");

            var hobbies = (await _usesTagRepository.GetHobbies())
                .Where(hobby => FixedHobbyNames.Contains(hobby.Name))
                .ToList();
            var categoryIds = selection.CategoryIds.Distinct().ToList();
            var tagIds = selection.TagIds.Distinct().ToList();
            var categories = hobbies.SelectMany(hobby => hobby.Categories.Select(category => (HobbyId: hobby.Id, Category: category))).ToList();
            var validCategoryHobbies = categories.ToDictionary(item => item.Category.Id, item => item.HobbyId);
            var validTagCategories = categories
                .SelectMany(item => item.Category.Tags.Select(tag => (TagId: tag.Id, CategoryId: item.Category.Id)))
                .ToDictionary(item => item.TagId, item => item.CategoryId);

            if (categoryIds.Any(id => !validCategoryHobbies.ContainsKey(id)) ||
                tagIds.Any(id => !validTagCategories.ContainsKey(id)) ||
                tagIds.Any(id => !categoryIds.Contains(validTagCategories[id])))
            {
                throw new ArgumentException("Selected hobbies, categories, and tags do not form a valid selection.");
            }

            await _usesTagRepository.SetUserTagSelection(userId, categoryIds, tagIds);
        }

        public async Task RemoveTagFromUser(int userId, List<AddTagToUserRequest> tag)
        {
            List<Database.Entities.Tag> tags = new List<Database.Entities.Tag>();
            foreach (var tagRequest in tag)
            {
                tags.Add(new Database.Entities.Tag { Id = tagRequest.Id });
            }
            await _usesTagRepository.RemoveTagFromUser(userId, tags);
        }
    }
}
