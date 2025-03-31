namespace Cogni.Models;

public class AuthedUserModel {
    public int Id {get; set;}
    public string Name {get; set;}
    public string Surname {get; set;}
    public string? Description {get; set;}
    public string? Image {get; set;}
    public string? BannerImage {get; set;}
    public string TypeMbti {get; set;}
    public string Role {get; set;}
    public int? LastLogin {get; set;}
    public string AccessToken {get; set;}
    public string RefreshToken {get; set;}
    public int AccessTokenExpireTime {get; set;}
    public int RefreshTokenExpiryTime {get; set;}
}