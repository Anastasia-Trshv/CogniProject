using Cogni.Models;

namespace Cogni.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> Get(string login, string passwordhash);//вход
        //Task<UserModel> Get(long id); //для профиля
        Task<long> Create(Customuser user);
        Task<bool> CheckUser(string login);//проверка существования пользователя с таким логином
        Task SetTestResult(UserModel user, int mbtiId);
    }
}
