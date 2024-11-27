using Cogni.Database.Context;
using Cogni.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CogniController : ControllerBase//временный контроллер
    {
        private readonly CogniDbContext _context;
        public CogniController(CogniDbContext cogniDb)
        {
            _context = cogniDb;
        }
        [HttpPost]
        public async Task<ActionResult<int>> CreateRole(string roleName, int id)
        {
            await _context.Roles.AddAsync(new Database.Entities.Role(id, roleName));
            _context.SaveChanges();
            return Ok(id);
        }
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetRoles()
        {
            List<Role> roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }
        [HttpPost]
        public async Task<ActionResult<int>> CreateMbti(string Name)
        {
            await _context.MbtiTypes.AddAsync(new MbtiType { NameOfType = Name });
            _context.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetMbtiTypes()
        {
            List<MbtiType> types = await _context.MbtiTypes.ToListAsync();
            return Ok(types);
        }
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllUsers()
        {
            List<User> users = await _context.Users.ToListAsync();
            return Ok(users);
        }
        [HttpPost]
        public async Task<ActionResult> CreateQuestion()
        {
            await _context.MbtiQuestions.AddRangeAsync(new MbtiQuestion[]
             {
            new MbtiQuestion { Question = "Почему у нас маскот это хомяк?" },
            new MbtiQuestion { Question = "Почему у нас маскот это хомяк?" },
            new MbtiQuestion { Question = "Почему у нас маскот это хомяк?" },
            new MbtiQuestion { Question = "Почему у нас маскот это хомяк?" },
            new MbtiQuestion { Question = "Почему у нас маскот это хомяк?" },
            new MbtiQuestion { Question = "Почему у нас маскот это хомяк?" }
             });
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> CreateTag()
        {
            await _context.Tags.AddRangeAsync(new Tag[]
             {
            new Tag {NameTag="Вязание"},
            new Tag {NameTag="Кино"},
            new Tag {NameTag="Аниме"},
            new Tag {NameTag="Гарри Поттер"},
            new Tag {NameTag="Лыжный спорт"},
            new Tag {NameTag="Коньки"},

             });
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> AddFriend()
        {
            await _context.AddRangeAsync(new Friend[]
            {
                new Friend{FriendId=1, UserId = 2, DateAdded=DateTime.Now},
                new Friend{FriendId=2, UserId = 3, DateAdded=DateTime.Now},
                new Friend{FriendId=3, UserId = 4, DateAdded=DateTime.Now},
                new Friend{FriendId=3, UserId = 9, DateAdded=DateTime.Now},
                new Friend{FriendId=3, UserId = 5, DateAdded=DateTime.Now},
            });
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<List<Friend>> GetAllFriend()
        {
            return await _context.Friends.ToListAsync();
        }
        //[HttpDelete]
        //public async Task RemoveFriend(int id1, int id2)
        //{
        //     var a = await _context.Friends.FindAsync(id);
        //     _context.Friends.Remove(a);
        //     await _context.SaveChangesAsync();
        //}
    }
}
