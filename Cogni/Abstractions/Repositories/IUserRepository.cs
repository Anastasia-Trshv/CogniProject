﻿using Cogni.Models;

namespace Cogni.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> Get(string login);//вход
        Task<UserModel> Get(int id); //получение всех данных для страницы профиля
        Task<UserModel> Create(User user);//создание
        Task<bool> CheckUser(string login);//проверка существования пользователя с таким логином
        Task SetMbtiType(int userid, int mbtiId);//установить результаты теста или изменить тип MBTY
        Task ChangeAvatar(int id,string picLink);//изменить автарарку
        Task ChangeBanner(int id, string picLink);//изменить баннер на странице пользователя
        Task<bool> ChangeName(int id, string name, string surname);//изменить имя
        Task ChangePassword(int id, string PasHash, byte[] salt);//изменить пароль
        Task<bool> ChangeDescription(int id, string description);//изменить описание
        Task<(string, DateTime, string)> GetRTokenAndExpiryTimeAndRole(long id);//возвращает данные для рефреша access токена
        Task RemoveTokens(int id); //удаляет токены пользователя(когда разлогиниваем) 
        Task UpdateUsersAToken(int id, string atoken);//обновляем аксес токен пользователя
        Task AddTokens(int id, string rtoken, string atoken, DateTime rTokenExpiry);
        Task<List<FriendDto>> GetRandomUsers(int userId, int startsFrom, int limit); //используется для получения случайных пользователей на странице с поиском друзей
        Task<List<FriendDto>> SearchUserByNameAndType(int userId, string NameSurname, int mbtiType);//поиск пользователей по типу и имени

    }
}
