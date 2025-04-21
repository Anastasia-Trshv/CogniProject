using Cogni.Abstractions.Services;
using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Cogni.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using Cogni.Authentication.Abstractions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.Swagger.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UserController(IUserService service, ITokenService tokenService) 
        {
            this._userService = service;
            this._tokenService = tokenService;
        }

        /// <summary>
        /// Используется при регистрации для проверки уникальности логина
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<bool>> ChekUser(string login)
        {
            return Ok(await _userService.ChekUser(login));
        }
        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <response code="200">Пользователь создан</response>
        /// <response code="404">Логин занят</response>
        [HttpPost]
        public async Task<ActionResult<AuthedUserModel>> CreateUser([FromBody] SignUpRequest request)
        {
            try
            {
                var user = await _userService.CreateUser(request);
                return Ok(user);
            }
            catch (DbUpdateException ex)
            {
                var msg = (ex.InnerException?.Message ?? ex.Message);
                if (msg.Contains("mbti_type", StringComparison.OrdinalIgnoreCase)) {
                    return BadRequest("Некорректный mbti!");
                }
            }
            catch (Exception ex) {
                if (ex.Message.Contains("exists", StringComparison.OrdinalIgnoreCase)) {
                    return BadRequest("Логин занят!");
                }
            }
            return StatusCode(500);
        }

        /// <summary>
        /// Вход пользователя в систему
        /// </summary>
        /// <response code="200">Пользователь найден, данные для входа верны</response>
        /// <response code="401">Неверный логин или пароль</response>
        [HttpPost]
        public async Task<ActionResult<AuthedUserModel>> LoginByEmail([FromBody] LoginRequest request)
        {
            var user = await _userService.Login(request.email, request.password);
            if (user == null) {
                return Unauthorized("Неверные данные для входа!");
            }
            return user;
        }

        /// <summary>
        /// Задает mbti тип пользователя
        /// </summary>
        /// <response code="200">Тип изменен</response>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult> SetTestResult([FromBody] SetTestResultRequest testRequest)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;
            // todo: VALIDATE REQUEST! IF EMPTY SEND, IT WILL SET TO DEFAULT!
            await _userService.SetMbtiType(id, testRequest.mbtiType.ToUpper());
            return Ok();
        }

        


        /// <summary>
        /// Меняет текущую аватарку на новую
        /// </summary>
        /// <remarks>Нужно в теле сообщения отправить файл(Key = Picture, multipart/form-data).  Поддерживаются форматы JPEG/JPG, PNG, GIF, BMP, TIFF, SVG, WebP, HEIF/HEIC, ICO, RAW. </remarks>
        [HttpPut]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> ChangeAvatar([FromForm] ImageUploadRequest request)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;
            var ava = await _userService.ChangeAvatar(id, request.Picture);
            return Ok(ava);
        }
        /// <summary>
        /// Меняет текущий баннер на новый
        /// </summary>
        /// <remarks>Нужно в теле сообщения отправить файл(Key = Picture, multipart/form-data). Поддерживаются форматы JPEG/JPG, PNG, GIF, BMP, TIFF, SVG, WebP, HEIF/HEIC, ICO, RAW.</remarks>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ChangeBanner([FromForm] ImageUploadRequest request)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;
            var ban = await _userService.ChangeBanner(id, request.Picture);
            return Ok(ban);
        }

        /// <summary>
        /// Изменяет описание пользователя
        /// </summary>
        /// <response code="200">Описание изменено</response>
        /// <response code="500">Что-то пошло не так</response>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<bool>> ChangeDescription([FromBody] ChangeDescriptionRequest descRequest)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;
            var res = await _userService.ChangeDescription (id, descRequest.Description);
            if (res)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Изменяет имя пользователя
        /// </summary>
        /// <response code="200">Имя изменено</response>
        /// <response code="500">Что-то пошло не так</response>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ChangeName([FromBody] ChangeNameRequest name)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;
            var res = await _userService.ChangeName(id, name.Name, name.Surname);
            if (res)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Изменяет пароль пользователя
        /// </summary>
        /// <response code="200">Пароль изменен</response>
        /// <response code="400">Старый пароль не совпадает</response>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ChangePassword( [FromBody] ChangePasswordRequest pasRequest)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;
            var result = await _userService.ChangePassword(id, pasRequest.OldPassword, pasRequest.NewPassword);
            if (result){
                return Ok();
            }
            else
            {
                return BadRequest("Old password is invalid");
            }
            
        }
        /// <summary>
        /// Возвращает общедоступные данные пользователя по id
        /// </summary>
        /// <response code="200">Пользователь найден</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PublicUserModel>> GetUserById(int id)
        {
            var user = await _userService.GetPublicUser(id);
            if (user == null) {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Возвращает случайные 25 пользователей
        /// </summary>
        /// <response code="200">Успешно</response>
        /// <remarks>startsFrom - скольких пользователей нужно пропустить(те которые уже были отправлены в предыдущих запросах), limit - скольких пользователей нужно отправить</remarks>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<FriendDto>>> GetRandomUsers(GetRandomUsersRequest request)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;
            return await _userService.GetRandomUsers(id, request.startsFrom, request.limit);
        }

        /// <summary>
        /// Поиск пользователей по имени и типу
        /// </summary>
        /// <response code="200">Успешно</response>
        /// <remarks></remarks>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<FriendDto>>> SearchUserByNameAndMbti(SearchUserRequest request)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;
            return await _userService.SearchUserByNameAndType(id, request.Name, request.mbtiType);
        }

        /// <summary>
        /// Получение нескольких пользователей по List<id>
        /// </summary>
        /// <remarks></remarks>
        [HttpPost]
        public async Task<List<PublicUserModel>> GetUsersByIds(List<int> userIds)
        {
            return await _userService.GetUsersByIds(userIds);
        }
    }
}

// Починка свагги
public class ImageUploadRequest
{
    [Required]
    public IFormFile Picture { get; set; }
}