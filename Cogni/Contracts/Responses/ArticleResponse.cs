namespace Cogni.Contracts
{
    public record ArticleResponse
    (
        int IdArticle,
        string ArticleName,
        string ArticleBody,
        List<string>? ImageUrls,
        int IdUser,
        string? Annotation,
        DateTime? Created,
        int? ReadsNumber
    );

}