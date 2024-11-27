using Cogni.Models;

namespace Cogni.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> Get(string login);//вход
        Task<UserModel> Get(int id); //получение всех данных для страницы профиля
        Task<int> Create(User user);//создание
        Task<bool> CheckUser(string login);//проверка существования пользователя с таким логином
        Task SetMbtiType(UserModel user, int mbtiId);//установить результаты теста или изменить тип MBTY
        Task ChangeAvatar(int id,string picLink);//изменить автарарку
        Task ChangeBanner(int id, string picLink);//изменить баннер на странице пользователя
        Task ChangeName(int id, string name);//изменить имя
        Task ChangePassword(int id, string PasHash, byte[] salt);//изменить пароль
        Task ChangeDescription(int id, string description);//изменить описание
        Task<(string, DateTime, string)> GetRTokenAndExpiryTimeAndRole(long id);//возвращает данные для рефреша access токена
        Task RemoveTokens(int id); //удаляет токены пользователя(когда разлогиниваем) 
        Task UpdateUsersAToken(int id, string atoken);//обновляем аксес токен пользователя
        Task AddTokens(int id, string rtoken, string atoken, DateTime rTokenExpiry);

    }
}
