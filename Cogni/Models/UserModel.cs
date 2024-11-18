namespace Cogni.Models
{
    public class UserModel
    {
        public int IdUser { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Image { get; set; }

        public string? TypeMbti { get; set; }

        public int IdRole { get; set; }

        public int IdMbtiType { get; set; }

        public DateTime? LastLogin { get; set; }

        public UserModel(int idUser, string? name, string? description, string? email, string? image, string? typeMbti, int idRole, int idMbtiType, DateTime? lastLogin)
        {
            IdUser = idUser;
            Name = name;
            Description = description;
            Email = email;
            Image = image;
            TypeMbti = typeMbti;
            IdRole = idRole;
            IdMbtiType = idMbtiType;
            LastLogin = lastLogin;
        }
        public UserModel() { }
    }
}
