using System.Linq;

namespace Kleu.Utility.Common
{
    public static class StringExtensions
    {
        public static string[] SplitClean(this string original, char separator)
        {
            if (string.IsNullOrEmpty(original))
                return new string[0];

            var result = original.Split(separator)
                .Select(part => part.Trim())
                .Where(part => !string.IsNullOrEmpty(part))
                .ToArray();

            return result;
        }
    }
}