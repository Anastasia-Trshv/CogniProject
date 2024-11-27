using System.Security.Claims;

namespace Cogni.Authentication.Abstractions
{
    public interface ITokenService
    {
        string GenerateAccessToken(int id, string role);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        DateTime GetRefreshTokenExpireTime();
        int GetIdFromToken(string token);
    }
}
