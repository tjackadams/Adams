using System.Linq;

namespace Adams.Core.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveWhiteSpace(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return source;
            }

            return new string(source.Where(s => !char.IsWhiteSpace(s)).ToArray());
        }
    }
}