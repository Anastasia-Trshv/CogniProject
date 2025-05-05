
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Cogni.Authentication;
public class AccessTokenPayload {
    public int UserId { get; set; }
    public string Role { get; set; }
    public AccessTokenPayload(int userId, string role)
    {
        UserId = userId;
        Role = role;
    }
    public ClaimsIdentity ToClaimsIdentity()
    {
        var claims = new List<Claim>
        {
            new Claim("userId", UserId.ToString()),
            new Claim(ClaimTypes.Role, Role)
        };

        return new ClaimsIdentity(claims, "jwt");
    }

    public static AccessTokenPayload FromClaimsPrincipal(ClaimsPrincipal principal)
    {
        var userIdClaim = principal.FindFirst("userId");
        var roleClaim = principal.FindFirst(ClaimTypes.Role);
        if (userIdClaim == null) return null;
        if (!int.TryParse(userIdClaim.Value, out int userId)) return null;
        if (roleClaim == null) return null;
        return new AccessTokenPayload(userId, roleClaim.Value);
    }
}

