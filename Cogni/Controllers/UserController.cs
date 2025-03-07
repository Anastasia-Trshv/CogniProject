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
        public async Task<ActionResult<FullUserResponse>> CreateUser([FromBody] SignUpRequest request)
        {
            var result = await _userService.CreateUser(request);
            if(result.Id == 0)
            {
                return BadRequest("Логин занят");
            }
            else{
                var newUser = new FullUserResponse(result.Id, result.Name, result.Surname, result.Description, result.ActiveAvatar, result.BannerImage, result.MbtyType, result.RoleName, result.LastLogin, result.AToken, result.RToken);
                return Ok(newUser);
            }
        }

        /// <summary>
        /// Вход пользователя в систему
        /// </summary>
        /// <response code="200">Пользователь найден, данные для входа верны</response>
        /// <response code="404">Неверный логин или пароль</response>
        [HttpPost]
        public async Task<ActionResult<FullUserResponse>> GetUserByEmail([FromBody] LoginRequest request)
        {
            var user = await _userService.GetUser(request.email, request.password);
            //конверация в userresponse
            if(user.Id == 0)
            {
                return NotFound();
            }
            else
            {
                var response = new FullUserResponse(user.Id, user.Name, user.Surname, user.Description, user.ActiveAvatar, user.BannerImage, user.MbtyType, user.RoleName, user.LastLogin, user.AToken, user.RToken);//создать токены

                return Ok(response);
            }
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
            int id = _tokenService.GetIdFromToken(token);
            // todo: VALIDATE REQUEST! IF EMPTY SEND, IT WILL SET TO DEFAULT!
            await _userService.SetMbtiType(id, testRequest.mbtiType.ToUpper());
            return Ok();
        }
        /// <summary>
        /// Меняет текущую аватарку на новую
        /// </summary>
        /// <remarks>Нужно в теле сообщения отправить файл(Key = Picture, multipart/form-data).  Разрешение изображения должно быть между 64x64 и 1024x1024. Изображения должны быть в формате JPG или PNG.  </remarks>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ChangeAvatar([FromForm] IFormFile Picture)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetIdFromToken(token);
            var ava = await _userService.ChangeAvatar(id, Picture);
            return Ok(ava);
        }
        /// <summary>
        /// Меняет текущий баннер на новый
        /// </summary>
        /// <remarks>Нужно в теле сообщения отправить файл(Key = Picture, multipart/form-data).  Разрешение изображения должно быть между 64x64 и 1024x1024. Изображения должны быть в формате JPG или PNG.  </remarks>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ChangeBanner([FromForm] IFormFile Picture)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetIdFromToken(token);
            var ban = await _userService.ChangeBanner(id, Picture);
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
            int id = _tokenService.GetIdFromToken(token);
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
            int id = _tokenService.GetIdFromToken(token);
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
            int id = _tokenService.GetIdFromToken(token);
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
        public async Task<ActionResult<UserByIdResponse>> GetUserById(int id)
        {
            var user = await _userService.Get(id);
            if(user.Id == 0)
            {
                return NotFound();
            }
            else
            {
                UserByIdResponse response = new UserByIdResponse(id, user.Name, user.Surname ,user.Description, user.ActiveAvatar, user.BannerImage, user.MbtyType, user.LastLogin);

                return Ok(response);
            }
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
            int id = _tokenService.GetIdFromToken(token);
            return await _userService.GetRandomUsers(id, request.startsFrom, request.limit);
        }


    }
}
