using System.Linq;

namespace Core.Runtime.Utils
{
    public static class StringUtils
    {
        public static string Random(uint length)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        
            return new string(Enumerable.Range(0, (int)length)
                .Select(_ => characters[System.Security.Cryptography.RandomNumberGenerator.GetInt32(characters.Length)])
                .ToArray());
        }
    }
}