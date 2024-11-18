using Cogni.Abstractions.Repositories;
using Cogni.Database.Context;
using Cogni.Models;
using Microsoft.EntityFrameworkCore;

namespace Cogni.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly CogniDbContext _context;
        public UserRepository(CogniDbContext context) 
        {
            _context = context;
        }

        public async Task<bool> CheckUser(string login)
        {
            Customuser? user = await _context.Customusers.FindAsync(login);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<long> Create(Customuser user)
        {
            await _context.Customusers.AddAsync(user);
            await _context.SaveChangesAsync();

            var us = await _context.Customusers.FirstAsync(l => user.Email == l.Email && user.Password == l.Password);
            return us.IdUser;
        }

        public async Task<UserModel> Get(string email, string passwordhash)
        {
           Customuser? user = await _context.Customusers.FirstOrDefaultAsync(u => u.Email == email && u.Password == passwordhash);
            if (user == null)
            {
                var newuser = new UserModel();
                return newuser;
            }
            else
            {
                UserModel newuser = Converter(user);
                return newuser;
            }

        }

        private UserModel Converter(Customuser user)//метод конвертирующие из User-сущности в UserModel 
        {
            return new UserModel(user.IdUser, user.Name, user.Description, user.Email, user.Image, user.TypeMbti, user.IdRole, user.IdMbtiType, user.LastLogin);
        }
    }
}
