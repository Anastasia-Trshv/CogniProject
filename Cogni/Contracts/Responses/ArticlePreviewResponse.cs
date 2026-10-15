namespace Cogni.Contracts.Responses
{
    public record ArticlePreviewResponse
    (
        int IdArticle,
        int IdUser,
        string? ArticlePreview, 
        string? UserProfilePicture, 
        string UserName,
        int ReadsNumber,
        string TimeSincePublication,
        string? UserMbti,
        string ArticleName,
        string? Annotation
    );
}
