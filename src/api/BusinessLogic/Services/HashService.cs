using BusinessLogic.Abstractions;

namespace BusinessLogic.Services
{
    public class HashService : IHashService
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public string Hash(int number, int length)
        {
            var rand = new Random(number);
            var result = "";

            for (int i = 0; i < length; i++)
            {
                result += chars[rand.Next() % chars.Length];
            }

            return result;
        }
    }
}
