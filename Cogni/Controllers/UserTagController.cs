using Cogni.Abstractions.Services;
using Cogni.Authentication.Abstractions;
using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Cogni.Services;
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
        private readonly ITokenService _tokenService;
        public UserTagController(IUserTagService userTagService, ITokenService tokenService)
        {
            _userTagService = userTagService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Связывает тэг с пользователем
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Authorize]
        public async Task<ActionResult> AddNewTagToUser(List<AddTagToUserRequest> tags)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;
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
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;
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
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;

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
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;
            await _userTagService.RemoveTagFromUser(id, tags);
            return Ok();
        }

    }
}
