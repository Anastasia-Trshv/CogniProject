namespace Cogni.Contracts.Requests
{
    public record CreateArticleRequest
    (

        string ArticleName,
        string ArticleBody,
        IFormFileCollection? Files,
        string Annotation
    );
}
