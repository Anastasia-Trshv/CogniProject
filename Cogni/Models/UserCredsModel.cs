namespace Cogni.Models;

public class UserCredsModel
{
    public int Id { get; set; }
    public string RoleName { get; set; }   
    public string PasswordHash { get; set; }
    public byte[] Salt { get; set; }
}