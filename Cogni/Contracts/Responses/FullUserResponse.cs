namespace Cogni.Contracts.Responses
{
    public record FullUserResponse
    (
        int Id,
        string Name,
        string Surname,
        string? Description,
        string? Image,
        string? BannerImage,
        string TypeMbti,
        string Role,
        DateTime? LastLogin,
        string? AToken,
        string? RToken
    );
}
