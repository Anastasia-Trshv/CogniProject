﻿using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
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
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Authorize]
        public async Task<ActionResult<int>> GetNumOfFriends(int id)
        {
            return Ok(_friendService.GetNumOfFriends(id));
        }
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
    }
}
