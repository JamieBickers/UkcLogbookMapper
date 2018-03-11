using System;
using System.Collections.Generic;
using System.Text;

namespace LocationMapper.WebScrapers
{
    internal static class Utilities
    {
        public static string SubstringOrDefault(this string input, int startIndex)
        {
            if (startIndex >= input.Length)
            {
                return null;
            }
            else
            {
                return input.Substring(startIndex);
            }
        }
    }
}
