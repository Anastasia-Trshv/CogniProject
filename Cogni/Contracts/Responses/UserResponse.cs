namespace Cogni.Contracts.Responses
{
    public record UserResponse
    (
        int Id,
        string Name,
        string Description,
        string Image,
        string TypeMbti,
        int IdRole,
        DateTime? LastLogin
    );
}
