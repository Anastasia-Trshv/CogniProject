namespace Cogni.Contracts.Responses
{
    public record UserByIdResponse
    (
        int Id,
        string Name,
        string Surname,
        string? Description,
        string? Image,
        string? BannerImage,
        string TypeMbti,
        DateTime? LastLogin
    );
}
