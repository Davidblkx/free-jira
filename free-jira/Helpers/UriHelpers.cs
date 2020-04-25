using System.Collections.Generic;
using System;
using System.Linq;
namespace FreeJira.Helpers
{
    public static class UriHelpers
    {
        /// <summary>
        /// Generates an URI from a list of strings
        /// </summary>
        /// <param name="uriList"></param>
        /// <returns></returns>
        public static Uri UriFromStrings(params string[] uriList) {
            return new Uri(uriList.JoinString('/'));
        }

        /// <summary>
        /// Return URI relative to current one
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static Uri GetRelativeUri(this Uri baseUri, params string[] parts) {
            var list = new List<string> { baseUri.AbsoluteUri };
            list.AddRange(parts);
            return new Uri(list.JoinString('/'));
        }
    }
}