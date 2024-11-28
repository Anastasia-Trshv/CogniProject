using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Authentication.Abstractions;
using Cogni.Contracts.Requests;
using Cogni.Database.Repositories;
using Cogni.Models;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace Cogni.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IMbtiService _mbtiService;
        public UserService(IUserRepository repo, ITokenService tokenService, IPasswordHasher passwordHasher, IMbtiService mbtiService)
        {
            _userRepository = repo;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _mbtiService = mbtiService;
        }

        public async Task ChangeAvatar(int id, string picLink)
        {
            //TODO: записать картинку на облако и вернуть ее url
            await _userRepository.ChangeAvatar(id, picLink);
        }

        public async Task ChangeBanner(int id, string picLink)
        {
            //TODO: записать картинку на облако и вернуть ее url
            await _userRepository.ChangeBanner(id, picLink);
        }

        public async Task<bool> ChangeDescription(int id, string description)
        {
            return await _userRepository.ChangeDescription(id, description);
        }

        public async Task<bool> ChangeName(int id, string name)
        {
             return await _userRepository.ChangeName(id, name);
        }

        public async Task<bool> ChangePassword(int id, string oldPassword, string newPassword)
        {
            var user = await _userRepository.Get(id);
            bool result = _passwordHasher.VerifyPassword(oldPassword, user.PasswordHash, user.Salt);
            if (result == true)
            {
                byte[] salt;
                var newHash = _passwordHasher.HashPassword(newPassword, out salt);    
                await _userRepository.ChangePassword(id,newHash, salt);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ChekUser(string email)
        {
            return await _userRepository.CheckUser(email);
        }

        public async Task<int> CreateUser(SignUpRequest user)
        {
            if (await _userRepository.CheckUser(user.Email) == false)//если пользователь с такой почтой еще не существует
            {

                byte[] salt;
                string passHash = _passwordHasher.HashPassword(user.Password, out salt);
                var typeid = await _mbtiService.GetMbtiTypeIdByName(user.MbtiType);

                User userEntity = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    PasswordHash = passHash,
                    Salt = salt,
                    IdRole = 1,
                    IdMbtiType = typeid
                };
                var newuser = await _userRepository.Create(userEntity);
                var rtoken = _tokenService.GenerateRefreshToken();
                var atoken = _tokenService.GenerateAccessToken(newuser.Id, newuser.RoleName);
                var time = _tokenService.GetRefreshTokenExpireTime();
                await _userRepository.AddTokens(newuser.Id, rtoken, atoken, time);
                return newuser.Id;
            }
            else
            {
                return 0;
            }
        }

        public async Task<UserModel> Get(int id)
        {
            return await _userRepository.Get(id);
        }

        public async Task<(string, DateTime, string)> GetRTokenAndExpiryTimeAndRole(long id)
        {
            return await _userRepository.GetRTokenAndExpiryTimeAndRole(id);    
        }

        public async Task<UserModel> GetUser(string email, string password)
        {
            
            var user =  await _userRepository.Get(email);
            if(user.Id == 0)
            {
                return new UserModel();
            }
            else if (_passwordHasher.VerifyPassword(password, user.PasswordHash, user.Salt))
            {
                return user;
            }
            else
            {
                return new UserModel();
            }
        }

        public async Task RemoveTokens(int id)
        {
           await _userRepository.RemoveTokens(id);
        }

        public async Task SetMbtiType(int userId, string mbtiType)
        {
            var typeId =await _mbtiService.GetMbtiTypeIdByName(mbtiType);
            await _userRepository.SetMbtiType(userId, typeId);
        }

        public async Task UpdateUsersAToken(int id, string atoken)
        {
           await _userRepository.UpdateUsersAToken(id, atoken);
        }
    }
}
