using System.Linq;
using System.Collections.Generic;

namespace FreeJira.Helpers
{
    public static class EnumerableHelpers
    {
        /// <summary>
        /// Join multiple strings and separate them by a char,
        /// the values are trim at start and end
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string JoinString(this IEnumerable<string> list, char separator = ';') {
            return string.Join(separator,
                list.Select(e => e.TrimEnd(separator).TrimStart(separator)));
        }
    }
}