namespace Cogni.Models
{
    public class PostModel
    {
        public int Id { get; set; }

        public string? PostBody { get; set; }

        public int IdUser { get; set; }

        public virtual ICollection<PostImage>? PostImages { get; set; } = new List<PostImage>();
    }
}
