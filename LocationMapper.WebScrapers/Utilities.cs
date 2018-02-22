using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LocationMapper.WebScrapers
{
    static class Utilities
    {
        // Extension method to get i'th element of a list, or default value if out of range.
        internal static T GetIndexOrDefault<T>(this List<T> list, int index)
        {
            if (index < list.Count())
            {
                return list[index];
            }
            else
            {
                return default(T);
            }
        }
    }
}
