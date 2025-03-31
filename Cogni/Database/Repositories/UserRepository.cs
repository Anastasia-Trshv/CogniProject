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

        readonly ILogger<UserRepository> _logger;


        public UserRepository(CogniDbContext context, ILogger<UserRepository> logger) 
        {
            _logger = logger;
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

        public async Task<PrivateUserModel> CreateUser(User user)
        {
            // C каких пор Email стал неуникальным?
            await _context.Users.AddAsync(user);
            //await _context.Avatars.AddAsync(new Avatar {UserId = user.Id, AvatarUrl = placeholder, IsActive=true, DateAdded=DateTime.Now});
            await _context.SaveChangesAsync();
            _context.Entry(user).Reference(u => u.IdRoleNavigation).Load();
            _context.Entry(user).Reference(u => u.IdMbtiTypeNavigation).Load();
            _context.Entry(user).Collection(u => u.Avatars).Load();
            PrivateUserModel model = Converter(user);
            return model;
        }

        public async Task<PrivateUserModel?> GetPrivateUserByEmail(string email){
            User? user = await _context.Users
                .Include(u => u.IdMbtiTypeNavigation)
                .Include(u => u.IdRoleNavigation)
                .Include(u => u.Avatars)
                .FirstOrDefaultAsync(u=> u.Email==email);
            if (user == null){
                return null;
            }
            return Converter(user);
        }

        public async Task<UserCredsModel?> GetUserCredsById(int id){
            User? user = await _context.Users.FirstOrDefaultAsync(u=> u.Id==id);
            if (user == null){
                return null;
            }
            return new UserCredsModel{Id = user.Id, PasswordHash = user.PasswordHash, Salt = user.Salt};
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
            // - ничего не делать)
            // todo: handle error
            return;
        }

        public async Task<PublicUserModel?> GetPublicUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.IdMbtiTypeNavigation)
                .Include(u => u.IdRoleNavigation)
                .Include(u => u.Avatars)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null){ return null;}
            var userModel = Converter(user);
            return userModel.ToPublic();
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

       
        public async Task<string?> GetUserRole(int id)
        {
            var role = await _context.Users
                .Include(u => u.IdRoleNavigation)
                .Where(u => u.Id == id)
                .Select(u => u.IdRoleNavigation.NameRole)
                .FirstOrDefaultAsync();
            return role;
        }

        public async Task<List<FriendDto>> GetRandomUsers(int userId, int startsFrom, int limit)
        {
            var users = _context.Users
                .OrderBy(u => u.Id)
                .Skip(startsFrom)
                .Take(limit)
                .Where(u => u.Id != userId)
                .Select(u => new
                {
                    u.Id,
                    u.Surname,
                    u.Name,
                    u.IdMbtiType,
                    Avatar = u.Avatars
                    .Where(a => a.IsActive == true)
                    .Select(u =>  u.AvatarUrl)
                    .FirstOrDefault()
                })
                .ToList();

            List<FriendDto> result = new List<FriendDto>();
            foreach(var u in users)
            {
                result.Add(new FriendDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    PicUrl = u.Avatar,
                    Mbti = u.IdMbtiType
                });
            } 
            return result;
        }

        public async Task<List<FriendDto>> SearchUserByNameAndType(int userId, string NameSurname, int mbtiType)
        {
            var users = _context.Users
                .OrderBy(u => u.Id)
                .Where(u => u.Id != userId && // Исключаем текущего пользователя
                (string.IsNullOrEmpty(NameSurname) || // Если NameSurname не указан, игнорируем это условие
                ((u.Name + " " + u.Surname).ToLower()).Contains((NameSurname.Trim()).ToLower())) && // Поиск по полному имени
                (mbtiType == 0 || u.IdMbtiType == mbtiType))
                .Select(u => new
                {
                    u.Id,
                    u.Surname,
                    u.Name,
                    u.IdMbtiType,
                    Avatar = u.Avatars
                    .Where(a => a.IsActive == true)
                    .Select(u => u.AvatarUrl)
                    .FirstOrDefault()
                })
                .ToList();

            List<FriendDto> result = new List<FriendDto>();
            foreach (var u in users)
            {
                result.Add(new FriendDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    PicUrl = u.Avatar,
                    Mbti = u.IdMbtiType
                });
            }
            return result;
        }
        private PrivateUserModel Converter(User user)//метод конвертирующие из User-сущности в PrivateUserModel 
        {
            Avatar? avatar = user.Avatars.FirstOrDefault(i => i.IsActive == true);
            return new PrivateUserModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Description = user.Description,
                Salt = user.Salt,
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                BannerImage = user.BannerImage,
                IdRole = user.IdRole,
                IdMbtiType = user.IdMbtiType,
                MbtiType = user.IdMbtiTypeNavigation.NameOfType,
                RoleName = user.IdRoleNavigation.NameRole,
                LastLogin = user.LastLogin,
                ActiveAvatar = avatar == null ? "" : avatar.AvatarUrl
            };
        }
    }
}
