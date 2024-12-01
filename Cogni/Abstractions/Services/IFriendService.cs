namespace Cogni.Abstractions.Services
{
    public interface IFriendService
    {
        Task<int> GetNumOfFriends(int userId);//возвращает количество друзей пользователя
        Task<List<Friend>> GetAllUserFriends(int userId);//возвращает всех друзей пользователя
        Task<List<(int id, string picUrl)>> GetFriendsPreview(int userId);//возвращает 6 аватарок пользователей для блока "Друзья" на странице профиля
    }
}
