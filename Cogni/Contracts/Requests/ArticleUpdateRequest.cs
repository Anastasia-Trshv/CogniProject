namespace Cogni.Contracts.Requests
{
    public record ArticleUpdateRequest
    (
        int IdArticle,
        string ArticleName,
        string ArticleBody,
        List<string> ImageUrls,
        int IdUser,
        IFormFileCollection? Files,
        string Annotation
    );
}
