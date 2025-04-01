using System.Security.Claims;

namespace Cogni.Authentication.Abstractions
{
    public interface ITokenService
    {
        string GenerateAccessToken(int id, string role);
        string GenerateRefreshToken(int id);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        DateTime GetRefreshTokenExpireTime();
        DateTime GetAccessTokenExpireTime();
        int GetIdFromToken(string token);
    }
}
