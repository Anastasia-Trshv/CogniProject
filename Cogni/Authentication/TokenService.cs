using Cogni.Authentication.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Cogni.Authentication;
public class TokenService : ITokenService
{
    private readonly IDatabase _redisDb;
    private readonly ITokenValidation _tokenValidation;

    public TokenService(IConfiguration config, IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
        _tokenValidation = new TokenValidation(config);
    }

    public string GenerateAccessToken(AccessTokenPayload payload)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = payload.ToClaimsIdentity(),
            Issuer = _tokenValidation.Issuer,
            Audience = _tokenValidation.Audience,
            Expires = DateTime.UtcNow.AddMinutes(AuthOptions.AccessTokenExpirationTime),
            SigningCredentials = _tokenValidation.GetSigningCredentials()
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken(int userId)
    {
        var refreshToken = Guid.NewGuid().ToString();
        _redisDb.StringSet($"refresh_token:{refreshToken}", userId.ToString(), 
                            TimeSpan.FromMinutes(AuthOptions.RefreshTokenExpirationTime));
        return refreshToken;
    }
    public AccessTokenPayload GetTokenPayload(string token, bool allowExpired=false){
        var p = _tokenValidation.GetPrincipalFromToken(token, allowExpired);
        return AccessTokenPayload.FromClaimsPrincipal(p);
    }

    public DateTime GetRefreshTokenExpireTime()
    {
        return DateTime.UtcNow.AddMinutes(AuthOptions.RefreshTokenExpirationTime);
    }

    public DateTime GetAccessTokenExpireTime()
    {
        return DateTime.UtcNow.AddMinutes(AuthOptions.AccessTokenExpirationTime);
    }
}
