using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cogni.Authentication;
public static class AuthOptions
{
    public const int AccessTokenExpirationTime = 10;
    public const int RefreshTokenExpirationTime = 2 * 24 * 60;
    public const string Algorithm = SecurityAlgorithms.HmacSha256; // TODO: use rsa256 instead
    public static SymmetricSecurityKey GetSymmetricSecurityKey(string Key)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}

