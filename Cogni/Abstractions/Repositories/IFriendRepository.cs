﻿namespace Cogni.Abstractions.Repositories
{
    public interface IFriendRepository
    {
        Task<int> GetNumOfFriends(int userId);//возвращает количество друзей пользователя
        Task<List<FriendDto>> GetUserFriends(int userId);//возвращает всех друзей пользователя
        Task<List<(int id, string picUrl)>> GetFriendsPreview(int userId);//возвращает 6 аватарок пользователей для блока "Друзья" на странице профиля
        Task Subscribe(int userId, int friendId);//подписка на пользователя
        Task Unsubscribe(int userId, int friendId);//отписка от пользователя
    }
}
