namespace License_Key_Shop_Web.Utils
{
    public class CodeGenerator
    {
        public static string GenerateRandomString(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            char[] randomChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                randomChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(randomChars);
        }
    }
}
