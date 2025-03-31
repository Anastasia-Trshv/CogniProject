using Cogni.Authentication.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Cogni.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly IDatabase _redisDb;
        private readonly string issuer;
        private readonly string audience;
        private readonly string key;
        public TokenService(IConfiguration config, IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
            issuer = config["Token:Issuer"];
            audience = config["Token:Audience"];
            key = config["Token:Key"];
        }

        public string GenerateAccessToken(int id, string role)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Role,role)
                }),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddMinutes(AuthOptions.AccessTokenExpirationTime),
                SigningCredentials = new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256) // TODO: use rsa256 instead
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

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)//метод позволяет проверить и извлечь информацию из JWT-токена, даже если он истек, при условии, что токен был правильно подписан известным ключом подписи и содержит valid идентификационные данные
        {
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidAudience = audience,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(key),
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public DateTime GetRefreshTokenExpireTime()
        {
            return DateTime.UtcNow.AddMinutes(AuthOptions.RefreshTokenExpirationTime);
        }

        public DateTime GetAccessTokenExpireTime()
        {
            return DateTime.UtcNow.AddMinutes(AuthOptions.AccessTokenExpirationTime);
        }

        public int GetIdFromToken(string token)
        {
            var claims = GetPrincipalFromExpiredToken(token);
            var idclaim = claims.FindFirst(ClaimTypes.NameIdentifier);
            int id = int.Parse(idclaim.Value);
            return id;
        }
    }
}
