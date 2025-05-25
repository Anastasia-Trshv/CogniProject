namespace Cogni.Contracts.Responses
{
    public record PostResponse
    (
        int Id,
        string? PostBody,
        DateTime? CreateDate,
        int IdUser,
        List<string>? PostImages
    );
}
