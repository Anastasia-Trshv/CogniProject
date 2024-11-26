using Cogni.Abstractions.Repositories;
using Cogni.Database.Context;
using Cogni.Database.Entities;
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
            if (await CheckUser(user.Email) == false)//если пользователь с такой почтой еще не существует
            {
                await _context.Customusers.AddAsync(user);
                await _context.SaveChangesAsync();

                var us = await _context.Customusers.FirstAsync(l => user.Email == l.Email && user.PasswordHash == l.PasswordHash);
                return us.IdUser;
            }
            else
            {
                return 0;
            }
           
        }

        public async Task<UserModel> Get(string email)
        {
           Customuser? user = await _context.Customusers
                .Include(u => u.Avatars)
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                var newuser = new UserModel();
                return newuser;
            }
            else
            {
                UserModel newuser = Converter(user);
                var pic  = user.Avatars.FirstOrDefault(r => r.IsActive == true);
                newuser.Image = pic.AvatarUrl;
                return newuser;
            }

        }
               
        public async Task SetMbtiType(UserModel user, int mbtiId)
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

        public async Task<UserModel> Get(int id)
        {
            var user = await _context.Customusers.FindAsync(id);
            return Converter(user);
        }

        public async Task ChangeAvatar(int id, string picLink)
        {
            var user = await _context.Customusers
                .Include(u => u.Avatars)
                .FirstOrDefaultAsync(u => u.IdUser== id);

            var avatar = user.Avatars.FirstOrDefault(r => r.IsActive == true);
            avatar.IsActive = false;

            user.Avatars.Add(new Avatar 
            { 
                AvatarUrl = picLink,
                UserId=id,
                IsActive=true,
                DateAdded = DateTime.Now,
            });

            await _context.SaveChangesAsync();  
        }

        public async Task ChangeBanner(int id, string picLink)
        {
            var user = await _context.Customusers.FindAsync(id);
            user.BannerImage = picLink;
        }

        public Task ChangeName(int id, string name)
        {
            throw new NotImplementedException();
        }

        public Task ChangePassword(int id, string PasHash, byte[] salt)
        {
            throw new NotImplementedException();
        }

        public Task ChangeDescription(int id, string description)
        {
            throw new NotImplementedException();
        }

        private UserModel Converter(Customuser user)//метод конвертирующие из User-сущности в UserModel 
        {
            return new UserModel(user.IdUser, user.Name, user.Description, user.Email, user.Image, user.IdRole, user.IdMbtiType, user.LastLogin);
        }

        public Task<(string, DateTime, string)> GetRTokenAndExpiryTimeAndRole(long id)
        {
            throw new NotImplementedException();
        }

        public Task<long> RemoveTokens(long id)
        {
            throw new NotImplementedException();
        }

        public Task<long> UpdateUsersAToken(long id, string atoken)
        {
            throw new NotImplementedException();
        }
    }
}
