using System;
using System.Linq;
using System.Security.Cryptography;

namespace WebApplication1.Data.Services
{
    public class HmacService
    {
        private static readonly byte[] _privateKey = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };
        private static readonly TimeSpan _passwordResetExpiry = TimeSpan.FromMinutes(5);
        private const byte _version = 1; // Increment this whenever the structure of the message changes.

        public static string CreatePasswordResetHmacCode(int userId)
        {
            byte[] message = Enumerable.Empty<byte>()
                .Append(_version)
                .Concat(BitConverter.GetBytes(userId))
                .Concat(BitConverter.GetBytes(DateTime.UtcNow.ToBinary()))
                .ToArray();

            using (HMACSHA256 hmacSha256 = new HMACSHA256(key: _privateKey))
            {
                byte[] hash = hmacSha256.ComputeHash(buffer: message, offset: 0, count: message.Length);

                byte[] outputMessage = message.Concat(hash).ToArray();
                string outputCodeB64 = Convert.ToBase64String(outputMessage);
                string outputCode = outputCodeB64.Replace('+', '-').Replace('/', '_');
                return outputCode;
            }
        }

        public static bool VerifyPasswordResetHmacCode(string codeBase64Url, string stringUserId)
        {
            if(codeBase64Url == null || stringUserId == null)
            {
                return false;
            }
            int userId = Convert.ToInt32(stringUserId);

            string base64 = codeBase64Url.Replace('-', '+').Replace('_', '/');
            byte[] message = Convert.FromBase64String(base64);

            byte version = message[0];
            if (version < _version)
            {
                return false;
            }

            if (userId != BitConverter.ToInt32(message, 1))
            {
                return false;
            }

            long createdUtcBinary = BitConverter.ToInt64(message, 5);
            DateTime createdUtc = DateTime.FromBinary(createdUtcBinary);

            if (createdUtc.Add(_passwordResetExpiry) < DateTime.UtcNow) return false;

            const int _messageLength = 1 + sizeof(int) + sizeof(long);

            using (HMACSHA256 hmacSha256 = new HMACSHA256(key: _privateKey))
            {
                byte[] hash = hmacSha256.ComputeHash(message, offset: 0, count: _messageLength);

                byte[] messageHash = message.Skip(_messageLength).ToArray();
                return Enumerable.SequenceEqual(hash, messageHash);
            }
        }
    }
}
