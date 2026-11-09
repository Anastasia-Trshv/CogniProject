using Cogni.Abstractions.Services;
using Cogni.Authentication.Abstractions;
using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ITokenService _tokenService;

        public PostController(IPostService postService, ITokenService tokenService)
        {
            _postService = postService;
            _tokenService = tokenService;
        }

        // Вспомогательный метод получения ID текущего пользователя
        private bool TryGetCurrentUserId(out int userId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("id")?.Value
                              ?? User.FindFirst("userId")?.Value;

            return int.TryParse(userIdClaim, out userId);
        }


        /// <summary>
        /// Создание поста на стене пользователя
        /// </summary>
        /// <remarks>Нужно в теле сообщения отправить файл/файлы (Key = Picture, multipart/form-data). Поддерживаются форматы JPEG/JPG, PNG, GIF, BMP, TIFF, SVG, WebP, HEIF/HEIC, ICO, RAW.</remarks>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Authorize]
        public async Task<ActionResult<PostResponse>> CreatePost([FromForm] PostRequest post)
        {
            /*string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;*/

            if (!TryGetCurrentUserId(out int userId))
            {
                return Unauthorized();
            }

            var p = await _postService.CreatePost(post, userId);

            /*var p = await _postService.CreatePost(post, id);*/

            var list = p.PostImages.ToList();
            List<string> urls = new List<string>();
            foreach (var image in list)
            {
                urls.Add(image.ImageUrl);
            }
            return Ok(new PostResponse(p.Id, p.PostBody,p.CreatedAt, p.IdUser, urls, 0, false));

        }
        /// <summary>
        /// Удаление поста со стены
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeletePost(int id)
        {
            if (!TryGetCurrentUserId(out int currentUserId))
            {
                return Unauthorized();
            }

            bool isAdmin = User.IsInRole("Admin");

            bool isDeleted = await _postService.DeletePost(
                id,
                currentUserId,
                isAdmin);

            if (!isDeleted)
            {
                return Forbid();
            }

            return Ok();
            /*await _postService.DeletePost(id);
            return Ok();*/
        }
        /// <summary>
        /// Получение всех постов пользователя
        /// </summary>
        /// 

  
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<PostResponse>>> GetAllUserPost([FromQuery] int id)
        {
            if (!TryGetCurrentUserId(out int currentUserId))
                return Unauthorized();

            var posts = await _postService.GetAllUserPosts(id);
            var list = new List<PostResponse>();

            foreach (var post in posts)
            {
                var urls = post.PostImages
                    .Select(u => u.ImageUrl)
                    .ToList();

                var reactionInfo = await _postService.GetReactionInfoAsync(
                    post.Id,
                    currentUserId);

                list.Add(new PostResponse(
                    post.Id,
                    post.PostBody,
                    post.CreatedAt,
                    post.IdUser,
                    urls,
                    reactionInfo.ReactionsCount,
                    reactionInfo.IsReactedByCurrentUser));
            }

            return Ok(list);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ToggleReaction(int postId, string reactionType = "like")
        {
            if (!TryGetCurrentUserId(out int userId))
                return Unauthorized();

            bool result = await _postService.ToggleReactionAsync(
                postId,
                userId,
                reactionType);

            if (!result)
                return NotFound();

            return Ok();
        }


    } 
}
