using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cogni.Authentication
{
    public static class AuthOptions
    {
        public const int AccessTokenExpirationTime = 30;
        public const int RefreshTokenExpirationTime = 2;
        public const string Issuer = "MyAuthServer";//TODO чекнуть что следует сюда вписывать
        public const string Audience = "MyAuthClient";
        const string Key //TODO перернести в конфигурационный файл
            = "Key1212121212121212154654987987813215613489789413_987135468791469874___usydygvaueyvbYFFYiugIUTRLTUfluyYFCYvuYFvTDFYTcytdOFcltydFulyf89745324986785641";
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        }
    }
}
