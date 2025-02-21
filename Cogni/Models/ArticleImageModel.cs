using System;

namespace Cogni.Models
{
    public class ArticleImageModel
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public string ImageUrl { get; set; } = null!;

        public ArticleImageModel(int id, int articleId, string imageUrl)
        {
            Id = id;
            ArticleId = articleId;
            ImageUrl = imageUrl;
        }

        public ArticleImageModel() { }
    }
}