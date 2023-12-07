using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace NEWS.Entities.Utils
{
    public static class CryptoUtils
    {
        public static string GenerateBase64Salt()
        {
            var salt = GetByteArray(8);
            return Convert.ToBase64String(salt);
        }

        public static string SHA256Crypt(string input)
        {
            var salt = GetByteArray(8);
            return SHA256Crypt(input, salt);
        }

        public static string SHA256Crypt(string input, string base64Salt)
        {
            byte[] salt = Convert.FromBase64String(base64Salt);
            return SHA256Crypt(input, salt);
        }

        public static string SHA256Crypt(string input, byte[] salt)
        {
            //var string1 = Convert.ToBase64String(salt);

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }

        private static byte[] GetByteArray(int size)
        {
            Random rnd = new Random();
            byte[] b = new byte[size];
            rnd.NextBytes(b);
            return b;
        }
    }
}
