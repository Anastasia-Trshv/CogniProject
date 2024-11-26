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

        public async Task SetTestResult(UserModel user, int mbtiId)
        {
            await _repository.SetMbtiType(user, mbtiId);
        }

    }
}
