namespace Cogni.Models;

public class PrivateUserModel
{
    public int Id {get; set;}
    public string Name {get; set;}
    public string Surname {get; set;}
    public string? Description {get; set;}
    public string? Image {get; set;}
    public string? BannerImage {get; set;}
    public string Role {get; set;}
    public DateTime? LastLogin {get; set;}
    public byte[] Salt {get; set;}
    public string? PasswordHash {get; set;}
    public string? Email {get; set;}
    public int IdRole {get; set;}
    public int IdMbtiType {get; set;}
    public string MbtiType {get; set;}
    public string? RoleName {get; set;}
    public string? ActiveAvatar {get; set;}

    public PublicUserModel ToPublic(){
        return new PublicUserModel{
            Id = this.Id,
            Name = this.Name,
            Surname = this.Surname,
            Description = this.Description,
            Image = this.Image,
            BannerImage = this.BannerImage,
            TypeMbti = this.MbtiType,
            LastLogin = this.LastLogin,
            ActiveAvatar = this.ActiveAvatar,
        };
    }

    public AuthedUserModel ToAuthed(
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpireTime,
        DateTime RefreshTokenExpiryTime
    ){
        return new AuthedUserModel{
            Id = this.Id,
            Name = this.Name,
            Surname = this.Surname,
            Description = this.Description,
            Image = this.Image,
            BannerImage = this.BannerImage,
            TypeMbti = this.MbtiType,
            Role = this.Role,
            LastLogin = this.LastLogin,
            AccessToken = AccessToken,
            RefreshToken = RefreshToken,
            AccessTokenExpireTime = AccessTokenExpireTime,
            RefreshTokenExpiryTime = RefreshTokenExpiryTime
        };
    }
}
