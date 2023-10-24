using System.Text;
using System.Security.Cryptography;

namespace License_Key_Shop_Web.Utils
{
    public class EncryptionMethods
    {
        public static string SHA256Encrypt(string inputString)
        {
            // Convert the input string to bytes
            byte[] data = Encoding.UTF8.GetBytes(inputString);

            // Create a new instance of the SHA-256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the bytes
                byte[] hashBytes = sha256.ComputeHash(data);

                // Convert the hash to a hexadecimal string
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashString;
            }
        }
    }
}
