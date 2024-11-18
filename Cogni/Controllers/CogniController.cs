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
    }
}
