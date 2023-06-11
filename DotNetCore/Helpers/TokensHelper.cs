using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace DotNetCore.Helpers
{
    public static class TokensHelper
    {
        public static string EncodeTokenFromString(string value)
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(value));
        }

        public static string DecodeTokenFromString(string token)
        {
            var bytes = WebEncoders.Base64UrlDecode(token);

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
