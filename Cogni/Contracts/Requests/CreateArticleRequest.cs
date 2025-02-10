namespace Cogni.Contracts.Requests
{
    public class CreateArticleRequest
    {
        public int IdArticle { get; set; }  // Добавлено свойство IdArticle
        public string ArticleName { get; set; }
        public string ArticleBody { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public int UserId { get; set; }
    }
}
