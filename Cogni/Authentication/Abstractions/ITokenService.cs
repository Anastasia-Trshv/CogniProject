using System.Security.Claims;

namespace Cogni.Authentication.Abstractions;
public interface ITokenService
{
    string GenerateAccessToken(AccessTokenPayload payload);
    string GenerateRefreshToken(int id);
    AccessTokenPayload GetTokenPayload(string token, bool allowExpired=false);
    DateTime GetRefreshTokenExpireTime();
    DateTime GetAccessTokenExpireTime();
}
