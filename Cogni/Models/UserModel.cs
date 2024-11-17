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
    }
}
