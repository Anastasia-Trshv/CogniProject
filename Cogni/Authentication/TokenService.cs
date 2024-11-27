using Cogni.Authentication.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Cogni.Authentication
{
    public class TokenService : ITokenService
    {
        const int RefressExpiryDays = 2;

        public string GenerateAccessToken(long id, string role)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Role,role)
                }),
                Issuer = AuthOptions.Issuer,
                Audience = AuthOptions.Audience,
                Expires = DateTime.Now.AddMinutes(AuthOptions.AccessTokenExpirationTime),
                SigningCredentials = new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)//метод позволяет проверить и извлечь информацию из JWT-токена, даже если он истек, при условии, что токен был правильно подписан известным ключом подписи и содержит valid идентификационные данные
        {
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = AuthOptions.Issuer,
                ValidAudience = AuthOptions.Audience,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.GetSymmetricSecurityKey().ToString())),
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
            return DateTime.UtcNow.AddDays(RefressExpiryDays);
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
