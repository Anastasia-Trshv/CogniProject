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

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UserController(IUserService service) 
        {
            this._userService = service;
        }

        
        [HttpGet]
        //используется при регистрации для проверки уникальности логина
        public async Task<ActionResult<bool>> ChekUser(string login)
        {
            return Ok(await _userService.ChekUser(login));
        }

        [HttpPost]
        public async Task<ActionResult<long>> CreateUser([FromBody] SignUpRequest request)
        {
            var result = await _userService.CreateUser(request);
            if(result == 0)
            {
                return BadRequest("Логин занят");
            }
            else{

                return Ok(result);
            }
        }


        [HttpPost]
        public async Task<ActionResult<UserResponse>> GetUserByLogin([FromBody] LoginRequest request)
        {
            var user = await _userService.GetUser(request.login, request.password);
            //конверация в userresponse
            var response = new UserResponse(user.Id, user.Name, user.Description, user.Image, user.BannerImage, user.MbtyType, user.RoleName, user.LastLogin, user.AToken, user.RToken);//создать токены

            return Ok(response);
        }

        [SwaggerOperation(Tags = new[] { "api" })]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult> SetTestResult(HttpRequest request, [FromBody] SetTestResultRequest testRequest)
        {
            int id = _tokenService.GetIdFromToken(request.Headers["Authorization"]);
            // todo: VALIDATE REQUEST! IF EMPTY SEND, IT WILL SET TO DEFAULT!
            // todo: get user from token?
            var user = new UserModel { Id = -1 };
            await _userService.SetMbtiType(user, testRequest.mbti_id);
            return Ok(testRequest.mbti_id);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ChangeAvatar(HttpRequest request, [FromHeader] ContentType content)
        {
            int id = _tokenService.GetIdFromToken(request.Headers["Authorization"]);
            await _userService.ChangeAvatar(id, "https://cache3.youla.io/files/images/780_780/5f/09/5f09f7160d4c733205084f38.jpg");
            return Ok();
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ChangeBanner(HttpRequest request, [FromHeader] ContentType content)
        {
            int id = _tokenService.GetIdFromToken(request.Headers["Authorization"]);
            await _userService.ChangeBanner(id, "https://cache3.youla.io/files/images/780_780/5f/09/5f09f7160d4c733205084f38.jpg");
            return Ok();
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ChangeDescription(HttpRequest request, [FromBody] ChangeDescriptionRequest descRequest)
        {
            int id = _tokenService.GetIdFromToken(request.Headers["Authorization"]);
            await _userService.ChangeDescription (id, descRequest.Description);
            return Ok();
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ChangeName(HttpRequest request, [FromBody] ChangeNameRequest name)
        {
            int id = _tokenService.GetIdFromToken(request.Headers["Authorization"]);
            await _userService.ChangeName(id, name.Name);
            return Ok();
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ChangePassword(HttpRequest request, [FromBody] ChangePasswordRequest pasRequest)
        {
            int id = _tokenService.GetIdFromToken(request.Headers["Authorization"]);
            var result = await _userService.ChangePassword(id, pasRequest.OldPassword, pasRequest.NewPassword);
            if (result){
                return Ok();
            }
            else
            {
                return BadRequest("Old password is invalid");
            }
            
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetUserById(HttpRequest request)
        {
            int id = _tokenService.GetIdFromToken(request.Headers["Authorization"]);
            var user = await _userService.Get(id);
            UserResponse response = new UserResponse(id, user.Name, user.Description, user.Image, user.BannerImage, user.MbtyType, user.RoleName, user.LastLogin, user.AToken, user.RToken);

            return Ok(response);
        }


    }
}
