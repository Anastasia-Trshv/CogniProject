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
                .Include(u => u.IdMbtiTypeNavigation)
                .Include(u => u.IdRoleNavigation)
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
                newuser.RoleName = user.IdRoleNavigation.NameRole;
                newuser.MbtyType = user.IdMbtiTypeNavigation.NameOfType;
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
            var user = await _context.Customusers
                .Include(u => u.IdMbtiTypeNavigation)
                .Include(u => u.IdRoleNavigation)
                .FirstOrDefaultAsync(u => u.IdUser == id);
            var userModel = Converter(user);
            userModel.RoleName = user.IdRoleNavigation.NameRole;
            userModel.MbtyType = user.IdMbtiTypeNavigation.NameOfType;
            return userModel;
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
            await _context.SaveChangesAsync();
        }

        public async Task ChangeName(int id, string name)
        {
            var user = await _context.Customusers.FindAsync(id);
            user.Name = name;
            await _context.SaveChangesAsync();
        }

        public async Task ChangePassword(int id, string PasHash, byte[] salt)
        {
            var user = await _context.Customusers.FindAsync(id);
            user.Salt = salt;
            user.PasswordHash = PasHash;
            await _context.SaveChangesAsync();
        }

        public async Task ChangeDescription(int id, string description)
        {
            var user = await _context.Customusers.FindAsync(id);
            user.Description = description;
            await _context.SaveChangesAsync();
        }

       
        public async Task<(string, DateTime, string)> GetRTokenAndExpiryTimeAndRole(long id)
        {
            var user = await _context.Customusers
               .Include(u => u.IdRoleNavigation)
               .FirstOrDefaultAsync(u => u.IdUser == id);
            return (user.RToken, user.RefreshTokenExpiryTime, user.IdRoleNavigation.NameRole);
        }

        public async Task RemoveTokens(int id)
        {
            var user = await _context.Customusers.FindAsync(id);
            user.AToken = null;
            user.RToken = null;
            user.RefreshTokenExpiryTime = default;
            await _context.SaveChangesAsync();
        }
        
        public async Task AddTokens(int id, string RToken, string AToken, DateTime expiry)
        {
            var user1 = await _context.Customusers.FindAsync(id);
            user1.AToken = AToken;
            user1.RToken = RToken;
            user1.RefreshTokenExpiryTime = expiry;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUsersAToken(int id, string atoken)
        {
            var user = await _context.Customusers.FindAsync(id);
            user.AToken = atoken;
            await _context.SaveChangesAsync();
        }
        private UserModel Converter(Customuser user)//метод конвертирующие из User-сущности в UserModel 
        {
            return new UserModel(user.IdUser, user.Name, user.Description, user.Email, user.Image, user.IdRole, user.IdMbtiType, user.LastLogin);
        }

    }
}
