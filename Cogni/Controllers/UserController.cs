using Microsoft.AspNetCore.Mvc;
using Cogni.Abstractions.Services;
using Cogni.Models;
using Cogni.Contracts.Requests;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> SetTestResult(SetTestResultRequest request)
        {
            // todo: VALIDATE REQUEST! IF EMPTY SEND, IT WILL SET TO DEFAULT!
            // todo: get user from token?
            var user = new UserModel { IdUser = -1 };
            await _userService.SetTestResult(user, request.mbti_id);
            return Ok(request.mbti_id);
        }
    }
}
