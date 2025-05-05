using Cogni.Models;
using Cogni.Database.Entities;
using Cogni.Contracts.Requests;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace Cogni.Abstractions.Services;
public interface IUserService
{
    Task<bool> ChekUser(string email);
    Task<AuthedUserModel> CreateUser(SignUpRequest user);
    Task<AuthedUserModel?> Login(string email, string password);
    Task<PublicUserModel?> GetPublicUser(int id); //получение всех данных для страницы профиля
    Task SetMbtiType(int userId, string mbtiType);
    Task<string> ChangeAvatar(int id, IFormFile picture);//изменить автарарку
    Task<string> ChangeBanner(int id, IFormFile picture);//изменить баннер на странице пользователя
    Task<bool> ChangeName(int id, string name, string surname);//изменить имя
    Task<bool> ChangePassword(int id, string oldPassword, string newPassword);//изменить пароль
    Task<bool> ChangeDescription(int id, string description);//изменить описание
    Task<string?> GetUserRole(int id);//возвращает роль пользователя для обновления access-токена при рефреше
    Task<List<FriendDto>> GetRandomUsers(int userId, int startsFrom, int limit);//используется для получения случайных пользователей на странице с поиском друзей
    Task<List<FriendDto>> SearchUserByNameAndType(int userId, string NameSurname, int mbtiType);//поиск друзей по типу и имени
    Task<List<PublicUserModel>> GetUsersByIds(List<int> userIds);
}
