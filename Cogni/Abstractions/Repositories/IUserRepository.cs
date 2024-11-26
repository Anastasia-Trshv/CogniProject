using Cogni.Models;

namespace Cogni.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> Get(string login);//вход
        Task<UserModel> Get(long id); //для профиля
        Task<int> Create(Customuser user);
        Task<bool> CheckUser(string login);//проверка существования пользователя с таким логином
        Task SetTestResult(UserModel user, int mbtiId);
        Task ChangeAvatar(string picLink);
        Task ChangeBanner(string picLink);
        Task ChangeName(string name);
        Task ChangePassword(string PasHash, byte[] salt);
        Task ChangeDescription(string description);
        
    }
}
