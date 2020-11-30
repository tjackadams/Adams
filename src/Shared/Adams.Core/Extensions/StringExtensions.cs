using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
