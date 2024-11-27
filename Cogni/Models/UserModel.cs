namespace Cogni.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public byte[] Salt { get; set; } = null!;

        public string? AToken { get; set; }

        public string? RToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? Image { get; set; }

        public string? BannerImage { get; set; }

        public int IdRole { get; set; }

        public int IdMbtiType { get; set; }
        
        public string? MbtyType { get; set; }

        public string? RoleName { get; set; }   

        public DateTime? LastLogin { get; set; }

        public UserModel(int idUser, string? name, string? description, string? email, string? image, int idRole, int idMbtiType, DateTime? lastLogin)
        {
            Id = idUser;
            Name = name;
            Description = description;
            Email = email;
            Image = image;
            IdRole = idRole;
            IdMbtiType = idMbtiType;
            LastLogin = lastLogin;
        }
        public UserModel() { }
    }
}
