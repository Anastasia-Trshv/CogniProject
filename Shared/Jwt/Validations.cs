using Microsoft.IdentityModel.Tokens;
using System.Text;
using Cogni.Authentication.Abstractions;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Cogni.Authentication;


// can be used both as independent class or with di
// TODO: Move to middleware!
public class TokenValidation : ITokenValidation {

    public string Issuer { get; private set; }
    public string Audience { get; private set; }
    public string Key { get; private set; }

    public TokenValidation(IConfiguration config)
    {
        Issuer = config["Token:Issuer"];
        Audience = config["Token:Audience"];
        Key = config["Token:Key"];
    }

    public TokenValidationParameters GetValidationParameters(bool allowExpired = false) {
        return new TokenValidationParameters
        {
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(Key),
            ValidateLifetime = !allowExpired
        };
    }
    
    public ClaimsPrincipal GetPrincipalFromToken(string token, bool allowExpired = false) {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("Token is null or empty");
        var parameters = GetValidationParameters(allowExpired);
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(AuthOptions.Algorithm, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }

    public AccessTokenPayload GetTokenPayload(string token, bool allowExpired=false){
        var p = GetPrincipalFromToken(token, allowExpired);
        return AccessTokenPayload.FromClaimsPrincipal(p);
    }

    public SigningCredentials GetSigningCredentials() {
        return new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(Key), AuthOptions.Algorithm);
    }
}
