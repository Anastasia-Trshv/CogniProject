namespace Cogni.Contracts.Responses
{
    public record PostResponse
    (
        int Id,
        string? PostBody,
        int IdUser,
        List<string>? PostImages
    );
}
