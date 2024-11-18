using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Contracts.Requests;
using Cogni.Models;

namespace Cogni.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
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
            //TODO: хэширование пароля
            Customuser userEntity = new Customuser(user.Name, user.Email, user.Password, 1, user.MbtiId);
            int id = await _repository.Create(userEntity);
            return id;
        }

        public async Task<UserModel> GetUser(string email, string passwordhash)
        {
            //TODO: хэширование пароля
            return await _repository.Get(email, passwordhash);
        }
    }
}
