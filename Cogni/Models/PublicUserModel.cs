namespace Cogni.Models;

public class PublicUserModel
{
    public int Id {get; set;}
    public string Name {get; set;}
    public string Surname {get; set;}
    public string? Description {get; set;}
    public string? Image {get; set;}
    public string? BannerImage {get; set;}
    public string TypeMbti {get; set;}
    public string? ActiveAvatar {get; set;}
    public int? LastLogin {get; set;}

}
