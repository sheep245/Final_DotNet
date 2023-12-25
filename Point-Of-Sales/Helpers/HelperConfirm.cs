using System.Composition;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.Globalization;
using System;

namespace Point_Of_Sales.Helpers
{
    public class HelperConfirm
    {
        public static string Generatelink(string username)
        {
            var timeExpired = DateTime.Now.AddMinutes(1).ToString("yyyyMMddHHmmss");
            var text = $"username={username}&expire={timeExpired}";
            var token = GenToken(text);

            return $"/Auth/Verify?{text}&token={token}";
        }


        public static bool VerifyLink(string username, string token, string expire)
        {
            if (!ValidExpire(expire))
            {
                return false;
            }

            var token_1 = GenToken($"username={username}&expire={expire}");

            if (!token.Equals(token_1))
            {
                return false;
            }

            return true;
        }

        private static string GenToken(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString().Substring(0, 8);
            }
        }

        private static bool ValidExpire(string timeStampe)
        {
            DateTime time = DateTime.ParseExact(timeStampe, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            if (DateTime.Now.Minute - time.Minute > 1)
            {
                return false;
            }
            return true;
        }

    }
}
