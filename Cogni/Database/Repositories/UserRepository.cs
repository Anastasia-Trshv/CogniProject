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
            Customuser? user = await _context.Customusers.FirstOrDefaultAsync(u=> u.Email==login);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<int> Create(Customuser user)
        {
            //user.IdRoleNavigation = await _context.Roles.FindAsync(user.IdRole);

            await _context.Customusers.AddAsync(user);
            await _context.SaveChangesAsync();

            var us = await _context.Customusers.FirstAsync(l => user.Email == l.Email && user.PasswordHash == l.PasswordHash);
            return us.IdUser;
        }

        public async Task<UserModel> Get(string email)
        {
           Customuser? user = await _context.Customusers.FirstOrDefaultAsync(u => u.Email == email);
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
               
        public async Task SetTestResult(UserModel user, int mbtiId)
        {
            var u = await _context.Customusers
                .FirstOrDefaultAsync(u => u.IdUser == user.Id);
            if (u != null)
            {
                u.IdMbtiType = mbtiId;
                await _context.SaveChangesAsync();
                return;
            }
            // ачо делать если нет юзера?
            // todo: handle error
            return;
        }

        public Task<UserModel> Get(long id)
        {
            throw new NotImplementedException();
        }

        public Task ChangeAvatar(string picLink)
        {
            throw new NotImplementedException();
        }

        public Task ChangeBanner(string picLink)
        {
            throw new NotImplementedException();
        }

        public Task ChangeName(string name)
        {
            throw new NotImplementedException();
        }

        public Task ChangePassword(string PasHash, byte[] salt)
        {
            throw new NotImplementedException();
        }

        public Task ChangeDescription(string description)
        {
            throw new NotImplementedException();
        }

        private UserModel Converter(Customuser user)//метод конвертирующие из User-сущности в UserModel 
        {
            return new UserModel(user.IdUser, user.Name, user.Description, user.Email, user.Image, user.IdRole, user.IdMbtiType, user.LastLogin);
        }
    }
}
