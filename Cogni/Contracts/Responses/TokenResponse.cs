namespace Cogni.Contracts.Responses
{
    public record TokenResponse
    (
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpireTime,
        DateTime RefreshTokenExpireTime
    );
}
