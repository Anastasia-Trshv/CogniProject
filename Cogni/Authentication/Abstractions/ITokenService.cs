using System.Security.Claims;

namespace Cogni.Authentication.Abstractions
{
    public interface ITokenService
    {
        string GenerateAccessToken(long id, string role);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        DateTime GetRefreshTokenExpireTime();
    }
}
