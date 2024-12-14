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
            User? user = await _context.Users.FirstOrDefaultAsync(u=> u.Email==login);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<UserModel> Create(User user)
        {
            //var placeholder = "https://www.mirgovorit.ru/static/users/default_profile_image.9acfe78b8e1c.png";

            await _context.Users.AddAsync(user);
            //await _context.Avatars.AddAsync(new Avatar {UserId = user.Id, AvatarUrl = placeholder, IsActive=true, DateAdded=DateTime.Now});
            await _context.SaveChangesAsync();
            _context.Entry(user).Reference(u => u.IdRoleNavigation).Load();
            _context.Entry(user).Reference(u => u.IdMbtiTypeNavigation).Load();
            UserModel model = Converter(user);
            //model.ActiveAvatar = placeholder;
            return model;
        }

        public async Task<UserModel> Get(string email)
        {
           User? user = await _context.Users
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
                if (pic != null)
                { 
                    newuser.ActiveAvatar = pic.AvatarUrl; 
                }
                else 
                { 
                    newuser.ActiveAvatar = "https://cache3.youla.io/files/images/780_780/5f/09/5f09f7160d4c733205084f38.jpg"; 
                }
               
                newuser.RoleName = user.IdRoleNavigation.NameRole;
                newuser.MbtyType = user.IdMbtiTypeNavigation.NameOfType;
                
                return newuser;
            }

        }
               
        public async Task SetMbtiType(int userId, int mbtiId)
        {
            var u = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
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
            var user = await _context.Users
                .Include(u => u.IdMbtiTypeNavigation)
                .Include(u => u.IdRoleNavigation)
                .Include(u=> u.Avatars)
                .FirstOrDefaultAsync(u => u.Id == id);
            var userModel = Converter(user);
            userModel.RoleName = user.IdRoleNavigation.NameRole;
            userModel.MbtyType = user.IdMbtiTypeNavigation.NameOfType;
            Task task = Task.Factory.StartNew(()=>  userModel.ActiveAvatar = user.Avatars.FirstOrDefault(i => i.IsActive == true).AvatarUrl);
            task.Wait();
            return userModel;
        }

        public async Task ChangeAvatar(int id, string picLink)
        {
            var user = await _context.Users
                .Include(u => u.Avatars)
                .FirstOrDefaultAsync(u => u.Id== id);

            var avatar = user.Avatars.FirstOrDefault(r => r.IsActive == true);
            if (avatar != null)
            {
                avatar.IsActive = false;
            }

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
            var user = await _context.Users.FindAsync(id);
            user.BannerImage = picLink;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ChangeName(int id, string name, string surname)
        {
            var user = await _context.Users.FindAsync(id);
            user.Name = name;
            user.Surname = surname;
            await _context.SaveChangesAsync();
            if(user.Name == name && user.Surname == surname)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task ChangePassword(int id, string PasHash, byte[] salt)
        {
            var user = await _context.Users.FindAsync(id);
            user.Salt = salt;
            user.PasswordHash = PasHash;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ChangeDescription(int id, string description)
        {
            var user = await _context.Users.FindAsync(id);
            user.Description = description;
            await _context.SaveChangesAsync();
            if (user.Description == description)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       
        public async Task<(string, DateTime, string)> GetRTokenAndExpiryTimeAndRole(long id)
        {
            var user = await _context.Users
               .Include(u => u.IdRoleNavigation)
               .FirstOrDefaultAsync(u => u.Id == id);
            return (user.RToken, user.RefreshTokenExpiryTime, user.IdRoleNavigation.NameRole);
        }

        public async Task RemoveTokens(int id)
        {
            var user = await _context.Users.FindAsync(id);
            user.AToken = null;
            user.RToken = null;
            user.RefreshTokenExpiryTime = default;
            await _context.SaveChangesAsync();
        }
        
        public async Task AddTokens(int id, string RToken, string AToken, DateTime expiry)
        {
            var user1 = await _context.Users.FindAsync(id);
            user1.AToken = AToken;
            user1.RToken = RToken;
            user1.RefreshTokenExpiryTime = expiry;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUsersAToken(int id, string atoken)
        {
            var user = await _context.Users.FindAsync(id);
            user.AToken = atoken;
            await _context.SaveChangesAsync();
        }
        private UserModel Converter(User user)//метод конвертирующие из User-сущности в UserModel 
        {
            return new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Description = user.Description,
                AToken = user.AToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                RToken = user.RToken,
                Salt = user.Salt,
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                BannerImage = user.BannerImage,
                IdRole = user.IdRole,
                IdMbtiType = user.IdMbtiType,
                MbtyType = user.IdMbtiTypeNavigation.NameOfType,
                RoleName = user.IdRoleNavigation.NameRole,
                LastLogin= user.LastLogin

            };
        }

    }
}
