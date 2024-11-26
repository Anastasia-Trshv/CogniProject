using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Authentication.Abstractions;
using Cogni.Contracts.Requests;
using Cogni.Models;
using System.Threading.Tasks;

namespace Cogni.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        public UserService(IUserRepository repo) 
        {
        _repository = repo;
        }

        public Task ChangeAvatar(int id, string picLink)
        {
            throw new NotImplementedException();
        }

        public Task ChangeBanner(int id, string picLink)
        {
            throw new NotImplementedException();
        }

        public Task ChangeDescription(int id, string description)
        {
            throw new NotImplementedException();
        }

        public Task ChangeName(int id, string name)
        {
            throw new NotImplementedException();
        }

        public Task ChangePassword(int id, string PasHash, byte[] salt)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChekUser(string email)
        {
            return await _repository.CheckUser(email);
        }

        public async Task<int> CreateUser(SignUpRequest user)
        {
            //TODO: добавление токенов(если будут токены)
            byte[] salt;
            string passHash = _passwordHasher.HashPassword(user.Password, out salt);
            Customuser userEntity = new Customuser
            {
                Name = user.Name,
                Email = user.Email,
                PasswordHash = passHash,
                Salt = salt,
                IdRole = 1,
                IdMbtiType = user.MbtiId
            };
            int id = await _repository.Create(userEntity);
            return id;
        }

        public Task<UserModel> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<(string, DateTime, string)> GetRTokenAndExpiryTimeAndRole(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> GetUser(string email, string password)
        {
            
            var user =  await _repository.Get(email);

            if (_passwordHasher.VerifyPassword(password, user.PasswordHash, user.Salt))
            {
                return user;
            }
            else
            {
                return new UserModel();
            }
        }

        public Task<long> RemoveTokens(long id)
        {
            throw new NotImplementedException();
        }

        public async Task SetMbtiType(UserModel user, int mbtiId)
        {
            await _repository.SetMbtiType(user, mbtiId);
        }

        public Task<long> UpdateUsersAToken(long id, string atoken)
        {
            throw new NotImplementedException();
        }
    }
}
