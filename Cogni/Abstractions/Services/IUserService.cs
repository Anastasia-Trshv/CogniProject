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
        Task SetMbtiType(UserModel user, int mbtiId);
        Task ChangeAvatar(int id, string picLink);//изменить автарарку
        Task ChangeBanner(int id, string picLink);//изменить баннер на странице пользователя
        Task ChangeName(int id, string name);//изменить имя
        Task ChangePassword(int id, string PasHash, byte[] salt);//изменить пароль
        Task ChangeDescription(int id, string description);//изменить описание
        Task<(string, DateTime, string)> GetRTokenAndExpiryTimeAndRole(long id);//возвращает данные для рефреша access токена
        Task<long> RemoveTokens(long id); //удаляет токены пользователя(когда разлогиниваем) 
        Task<long> UpdateUsersAToken(long id, string atoken);//обновляем аксес токенпользователя
    }

}

