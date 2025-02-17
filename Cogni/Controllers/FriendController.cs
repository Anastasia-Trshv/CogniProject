using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
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
        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
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
        public async Task<ActionResult> Subscribe(int userId, int friendId)
        {
            // todo! : check token and read user id from it
            if (userId == friendId)
            {
                return BadRequest("Нельзя подписаться на самого себя");
            }
            try
            {
                await _friendService.Subscribe(userId, friendId);
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
            }
            return Ok();
            
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Authorize]
        public async Task<ActionResult> Unsubscribe(int userId, int friendId)
        {
            // todo! : check token and read user id from it
            try {
                await _friendService.Unsubscribe(userId, friendId);
                return Ok();
            } catch (Exception _) {}
            return StatusCode(500, "Неизвестная ошибка");

        }
    }
}
