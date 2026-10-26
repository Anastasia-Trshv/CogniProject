using Cogni.Abstractions.Services;
using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Cogni.Services;
using Cogni.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserTagController : ControllerBase
    {
        private readonly IUserTagService _userTagService;
        public UserTagController(IUserTagService userTagService)
        {
            _userTagService = userTagService;
        }

        /// <summary>
        /// Связывает тэг с пользователем
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Authorize]
        public async Task<ActionResult> AddNewTagToUser(List<AddTagToUserRequest> tags)
        {
            if (User.GetUserId() is not int id)
            {
                return Unauthorized();
            }
            await _userTagService.AddNewTagToUser(id, tags);
            return Ok();
        }

        /// <summary>
        /// Возвращает все тэги пользователя
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Authorize]
        public async Task<ActionResult<List<TagResponse>>> GetUserTags()
        {
            if (User.GetUserId() is not int id)
            {
                return Unauthorized();
            }
            var tags = await _userTagService.GetUserTags(id);
            List<TagResponse> result = new List<TagResponse>();
            foreach (var tag in tags)
            {
                result.Add(new TagResponse( tag.Id, tag.NameTag ));
            }
            return Ok(result); 
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult<List<HobbyResponse>>> GetTagCatalog()
        {
            return Ok(await _userTagService.GetHobbyCatalog());
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Authorize]
        public async Task<ActionResult<UserTagSelectionResponse>> GetUserTagSelection(int userId)
        {
            return Ok(await _userTagService.GetUserTagSelection(userId));
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        [Authorize]
        public async Task<ActionResult> SetUserTagSelection(SetUserTagSelectionRequest selection)
        {
            if (User.GetUserId() is not int id)
            {
                return Unauthorized();
            }

            try
            {
                await _userTagService.SetUserTagSelection(id, selection);
                return Ok();
            }
            catch (ArgumentException error)
            {
                return BadRequest(error.Message);
            }
        }

        /// <summary>
        /// Отвязывает тэг от пользователя
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Authorize]
        public async Task<ActionResult> RemoveTagFromUser(List<AddTagToUserRequest> tags)
        {
            if (User.GetUserId() is not int id)
            {
                return Unauthorized();
            }
            await _userTagService.RemoveTagFromUser(id, tags);
            return Ok();
        }

    }
}
