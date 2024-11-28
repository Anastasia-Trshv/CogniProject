using Cogni.Models;
using Cogni.Database.Entities;
using Cogni.Contracts.Requests;
using System.Threading.Tasks;

namespace Cogni.Abstractions.Services
{
    public interface IUserService
    {
        Task<bool> ChekUser(string email);
        Task<int> CreateUser(SignUpRequest user);
        Task<UserModel> GetUser(string email, string password);
        Task<UserModel> Get(int id); //получение всех данных для страницы профиля
        Task SetMbtiType(int userId, string mbtiType);
        Task ChangeAvatar(int id, string picLink);//изменить автарарку
        Task ChangeBanner(int id, string picLink);//изменить баннер на странице пользователя
        Task<bool> ChangeName(int id, string name);//изменить имя
        Task<bool> ChangePassword(int id, string oldPassword, string newPassword);//изменить пароль
        Task<bool> ChangeDescription(int id, string description);//изменить описание
        Task<(string, DateTime, string)> GetRTokenAndExpiryTimeAndRole(long id);//возвращает данные для рефреша access токена
        Task RemoveTokens(int id); //удаляет токены пользователя(когда разлогиниваем) 
        Task UpdateUsersAToken(int id, string atoken);//обновляем аксес токенпользователя
    }

}

