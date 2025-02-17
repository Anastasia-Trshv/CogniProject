namespace Cogni.Contracts
{
    public record ArticleResponse
    (
        int IdArticle,
        string ArticleName,
        string ArticleBody,
        List<string> ImageUrls,
        int IdUser
    );
    public record ArticlesResponse
    (
        int IdArticle,
        string ArticleName
    );
}