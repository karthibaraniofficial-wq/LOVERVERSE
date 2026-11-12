using System;
using System.Security.Cryptography;
using System.Text;

namespace Loveverse.Networking
{
    /// <summary>
    /// Cryptographically secure generator for 6-character room invitation codes.
    /// Excludes ambiguous characters (0, O, 1, I, L) for mobile keyboard friendliness.
    /// </summary>
    public static class RoomCodeGenerator
    {
        private const string Alphabet = "23456789ABCDEFGHJKMNPQRSTUVWXYZ";
        private const int CodeLength = 6;

        public static string Generate()
        {
            var bytes = new byte[CodeLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            var sb = new StringBuilder(CodeLength);
            for (int i = 0; i < CodeLength; i++)
            {
                int index = bytes[i] % Alphabet.Length;
                sb.Append(Alphabet[index]);
            }

            return sb.ToString();
        }

        public static bool IsValidCodeFormat(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length != CodeLength)
            {
                return false;
            }

            code = code.ToUpperInvariant();
            for (int i = 0; i < code.Length; i++)
            {
                if (Alphabet.IndexOf(code[i]) < 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
