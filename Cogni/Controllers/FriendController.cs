using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Authentication.Abstractions;
using Cogni.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Web.Http;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;
        private readonly ITokenValidation _tokenValidation; 
        public FriendController(IFriendService friendService, ITokenValidation tokenValidation)
        {
            _friendService = friendService;
            _tokenValidation = tokenValidation;
        }
        /// <summary>
        /// Получение количества друзей пользователя по id
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Authorize]
        public async Task<ActionResult<int>> GetNumOfFriends(int id)
        {
            return Ok(await _friendService.GetNumOfFriends(id));
        }
        /// <summary>
        /// Возвращает 6 фото и 6 id пользователй для отображения на странице профиля
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Authorize]
        public async Task<ActionResult<List<FriendsPreviewResponse>>> GetFriendsPreview(int id)
        {
            var list = await _friendService.GetFriendsPreview(id);
            var response = new List<FriendsPreviewResponse>();
            foreach (var l in list)
            {
                response.Add(new FriendsPreviewResponse(l.id, l.picUrl));
            }
            return Ok(response);
        }


        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Authorize]
        public async Task<ActionResult<List<(int id, string picUrl, string mbti)>>> GetUserFriends(int id)
        {
            var list = await _friendService.GetUserFriends(id);

            return Ok(list);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Authorize]
        public async Task<ActionResult> Subscribe(int friendId)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var payload = _tokenValidation.GetTokenPayload(token);
                var userId = payload.UserId;
                if (userId == friendId)
                {
                    return BadRequest("Нельзя подписаться на самого себя");
                }
                await _friendService.Subscribe(userId, friendId);
                return Ok();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e) when (e.InnerException is PostgresException pgEx)
            {
                switch (pgEx.SqlState)
                {
                    case "23505": // Unique constraint violation (duplicate entry)
                        return Conflict("Вы уже подписаны на этого пользователя");
                    
                    case "23503": // Foreign key violation (invalid reference)
                        return BadRequest("Пользователя с таким id не существует");

                    default:
                        return StatusCode(500, "Неизвестная ошибка");
                }
            } catch (Exception e) {
                return Unauthorized();
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Authorize]
        public async Task<ActionResult> Unsubscribe(int friendId)
        {
            try {
                var token = Request.Headers["Authorization"];
                var payload = _tokenValidation.GetTokenPayload(token);
                var userId = payload.UserId;
                await _friendService.Unsubscribe(userId, friendId);
                return Ok();
            } catch (Exception e) {
                return Unauthorized();
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Authorize]
        public async Task<ActionResult> IsSubscribed(int friendId)
        {
            try {
                var token = Request.Headers["Authorization"];
                var payload = _tokenValidation.GetTokenPayload(token);
                var userId = payload.UserId;
                return Ok(await _friendService.IsSubscribed(userId, friendId));
            } catch (Exception e) {
                return Unauthorized();
            }
        }
    }
}
