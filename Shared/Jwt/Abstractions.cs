
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
namespace Cogni.Authentication.Abstractions;

public interface ITokenValidation {
    string Issuer { get; }
    string Audience { get; }
    string Key { get; }
    public TokenValidationParameters GetValidationParameters(bool allowExpired = false);
    public ClaimsPrincipal GetPrincipalFromToken(string token, bool allowExpired = false);
    public AccessTokenPayload GetTokenPayload(string token, bool allowExpired=false);
    public SigningCredentials GetSigningCredentials();
}
