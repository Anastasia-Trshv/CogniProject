using Cogni.Models;
using Cogni.Database.Entities;
using Cogni.Contracts.Requests;

namespace Cogni.Abstractions.Services
{
    public interface IUserService
    {
        Task<bool> ChekUser(string email);

        Task<int> CreateUser(SignUpRequest user);

        Task<UserModel> GetUser(string email, string passwordhash);
    }
}
