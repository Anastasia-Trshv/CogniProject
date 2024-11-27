namespace Cogni.Contracts.Responses
{
    public record UserByIdResponse
    (
        int Id,
        string Name,
        string? Description,
        string? Image,
        string? BannerImage,
        string TypeMbti,
        DateTime? LastLogin
    );
}
