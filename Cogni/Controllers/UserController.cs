using Cogni.Abstractions.Services;
using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Cogni.Models;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService service;
        public UserController(IUserService service) 
        {
            this.service = service;
        }
        [HttpGet]
        //используется при регистрации для проверки уникальности логина
        public async Task<ActionResult<bool>> ChekUser(string login)
        {
            return Ok(await service.ChekUser(login));
        }

        [HttpPost]
        public async Task<ActionResult<long>> CreateUser([FromBody] SignUpRequest request)
        {

            return Ok(await service.CreateUser(request));
        }


        [HttpPost]
        public async Task<ActionResult<UserResponse>> GetUserByLogin([FromBody] LoginRequest request)
        {
            var user = await service.GetUser(request.login, request.password);
            //конверация в userresponse
            var response = new UserResponse(user.IdUser, user.Name, user.Description, user.Image, user.IdMbtiType.ToString(), user.IdRole, user.LastLogin);//создать токены

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SetTestResult(SetTestResultRequest request)
        {
            // todo: VALIDATE REQUEST! IF EMPTY SEND, IT WILL SET TO DEFAULT!
            // todo: get user from token?
            var user = new UserModel { IdUser = -1 };
            await service.SetTestResult(user, request.mbti_id);
            return Ok(request.mbti_id);
        }

        
    }
}
