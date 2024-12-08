using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Control.Panel.Model
{
    public class PasswordHashHandler
    {
        private static int _iterationCount = 100000;
        private static RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();

        public static string HashPassword(String password)
        {
            int saltSize = 128 / 8;
            var salt = new byte[saltSize];
            _randomNumberGenerator.GetBytes(salt);
            var subkey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, _iterationCount, 256 / 8);
            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01;
            WriteNetworkByteOrder(outputBytes, 1, (int)KeyDerivationPrf.HMACSHA512);
            WriteNetworkByteOrder(outputBytes, 5, _iterationCount);
            WriteNetworkByteOrder(outputBytes, 9, saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, +salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13, +subkey.Length);
            return Convert.ToBase64String(outputBytes);
        }
        public static bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = Convert.FromBase64String(hash);
            var keyDerivationProf = ReadNetworkByByteOrder<KeyDerivationPrf>(hashedPassword, 1);
            var iterationCount = (int)ReadNetworkByByteOrder<KeyDerivationPrf>(hashedPassword, 5);
            var saltLength = (int)ReadNetworkByByteOrder<KeyDerivationPrf>(hashedPassword, 9);
            if (saltLength < 128 / 8)
                return false;
            return true;
        }

        public static T ReadNetworkByByteOrder<T>(byte[] buffer, int offset)
        {
            if (typeof(T) == typeof(KeyDerivationPrf))
            {
                // Assuming you want to read an integer from the buffer and convert it to KeyDerivationPrf
                int value = BitConverter.ToInt32(buffer, offset);
                return (T)(object)(KeyDerivationPrf)value;
            }
            throw new InvalidOperationException("Unsupported type.");
        }
        public static void WriteNetworkByteOrder(byte[] buffer, int offset, int value)
        {
            // Convert the integer value to a byte array in big-endian order (network byte order)
            byte[] bytes = BitConverter.GetBytes(value);

            // If the system is little-endian, reverse the byte array
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            // Copy the bytes to the target buffer at the specified offset
            Array.Copy(bytes, 0, buffer, offset, bytes.Length);
        }
    }
}
