using ChatService.Database.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


/* 
      .o8                                                      oooo
     "888                                                      `888
 .oooo888   .ooooo.  oooo    ooo          .ooooo.  ooo. .oo.    888  oooo    ooo
d88' `888  d88' `88b  `88.  .8'          d88' `88b `888P"Y88b   888   `88.  .8'
888   888  888ooo888   `88..8'   8888888 888   888  888   888   888    `88..8'
888   888  888    .o    `888'            888   888  888   888   888     `888'
`Y8bod88P" `Y8bod8P'     `8'             `Y8bod8P' o888o o888o o888o     .8'
                                                                     .o..P'
                                                                     `Y8P'

*/


namespace ChatService.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IHubContext<ChatHubController> _hubContext;

    public AuthController(IHubContext<ChatHubController> hubContext, AppDbContext dbContext)
    {
        _hubContext = hubContext;
        _dbContext = dbContext;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (existingUser != null)
        {
            return Ok(new 
            {
                UserId = existingUser.Id,
                Username = existingUser.Username
            });
        }

        var user = new Entities.User // Use the Entities.User class here
        {
            Id = Guid.NewGuid(),
            Username = request.Username
        };

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return Ok(new 
        {
            UserId = user.Id,
            Username = user.Username
        });
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _dbContext.Users.ToListAsync();
        return Ok(users);
    }
}
