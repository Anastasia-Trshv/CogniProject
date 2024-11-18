using Cogni.Abstractions.Services;
using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

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
            var response = new UserResponse(user.IdUser, user.Name, user.Description, user.Image, user.TypeMbti, user.IdRole, user.LastLogin);//создать токены

            return Ok(response);
        }
    }
}
