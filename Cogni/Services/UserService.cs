using Cogni.Abstractions.Repositories;
using Cogni.Abstractions.Services;
using Cogni.Authentication.Abstractions;
using Cogni.Contracts.Requests;
using Cogni.Database.Repositories;
using Cogni.Models;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Cogni.Database.Entities;
using Cogni.Authentication;

namespace Cogni.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IMbtiService _mbtiService;
    private readonly IImageService _imageService;
    public UserService(IUserRepository repo, ITokenService tokenService, IPasswordHasher passwordHasher, IMbtiService mbtiService, IImageService imageService)
    {
        _userRepository = repo;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _mbtiService = mbtiService;
        _imageService = imageService;
    }

    public async Task<string> ChangeAvatar(int id, IFormFile picture)
    {
        var picLink = await _imageService.UploadImage(picture);
        await _userRepository.ChangeAvatar(id, picLink);
        return picLink;
    }

    public async Task<string> ChangeBanner(int id, IFormFile picture)
    {
        var picLink = await _imageService.UploadImage(picture);
        await _userRepository.ChangeBanner(id, picLink);
        return picLink; 
    }

    public async Task<bool> ChangeDescription(int id, string description)
    {
        return await _userRepository.ChangeDescription(id, description);
    }

    public async Task<bool> ChangeName(int id, string name, string surname)
    {
            return await _userRepository.ChangeName(id, name, surname);
    }

    public async Task<bool> ChangePassword(int id, string oldPassword, string newPassword)
    {
        var user = await _userRepository.GetUserCredsById(id);
        if (user != null && _passwordHasher.VerifyPassword(oldPassword, user.PasswordHash, user.Salt))
        {
            byte[] salt;
            var newHash = _passwordHasher.HashPassword(newPassword, out salt);    
            await _userRepository.ChangePassword(id,newHash, salt);
            return true;
        }
        return false;
    }

    public async Task<bool> ChekUser(string email)
    {
        return await _userRepository.CheckUser(email);
    }

    public async Task<AuthedUserModel> CreateUser(SignUpRequest user)
    {
        if (await _userRepository.CheckUser(user.Email) == false) //если пользователь с такой почтой еще не существует
        {
            byte[] salt;
            string passHash = _passwordHasher.HashPassword(user.Password, out salt);
            var typeid = await _mbtiService.GetMbtiTypeIdByName(user.MbtiType);
            User userEntity = new User
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PasswordHash = passHash,
                Salt = salt,
                IdRole = 1,
                IdMbtiType = typeid,
                LastLogin = DateTime.Now
            };
            var newuser = await _userRepository.CreateUser(userEntity);
            return newuser.ToAuthed(
                _tokenService.GenerateAccessToken(new AccessTokenPayload(newuser.Id, newuser.RoleName)),
                _tokenService.GenerateRefreshToken(newuser.Id),
                _tokenService.GetRefreshTokenExpireTime(),
                _tokenService.GetAccessTokenExpireTime()
            );
        }
        else
        {
            throw new Exception("email_exists"); // todo: сделать кастомные ошибки?
        }
    }

    public async Task<PublicUserModel?> GetPublicUser(int id) {
        return await _userRepository.GetPublicUser(id);
    }

    public async Task<string?> GetUserRole(int id)
    {
        return await _userRepository.GetUserRole(id);    
    }

    public async Task<AuthedUserModel?> Login(string email, string password)
    {
        var user =  await _userRepository.GetPrivateUserByEmail(email);
        if(user != null && _passwordHasher.VerifyPassword(password, user.PasswordHash, user.Salt))
        {
            return user.ToAuthed(
                _tokenService.GenerateAccessToken(new AccessTokenPayload(user.Id, user.RoleName)),
                _tokenService.GenerateRefreshToken(user.Id),
                _tokenService.GetAccessTokenExpireTime(),
                _tokenService.GetRefreshTokenExpireTime()
            );
        }
        return null;
    }

    public async Task SetMbtiType(int userId, string mbtiType)
    {
        var typeId = await _mbtiService.GetMbtiTypeIdByName(mbtiType);
        await _userRepository.SetMbtiType(userId, typeId);
    }
    
    public async Task<List<FriendDto>> GetRandomUsers(int userId, int startsFrom, int limit)
    {
        return await _userRepository.GetRandomUsers(userId, startsFrom, limit);
    }

    public async Task<List<FriendDto>> SearchUserByNameAndType(int userId, string NameSurname, int mbtiType)
    {
        return await _userRepository.SearchUserByNameAndType(userId, NameSurname, mbtiType);
    }

    public async Task<List<PublicUserModel>> GetUsersByIds(List<int> userIds)
    {
        if (userIds.Count == 0) return new List<PublicUserModel>();
        return await _userRepository.GetUsersByIds(userIds);
    }
}
