using System;
using ConsoleInteractive.InputConverter;

namespace FreeJira.Terminal.Converters
{
    public class UriConverter : StringConverter<Uri?>
    {
        public UriConverter() : base(UriToString, StringToUri) {}

        public static string UriToString(Uri? u) => u?.ToString() ?? "";
        public static Uri? StringToUri(string s) {
            if (Uri.IsWellFormedUriString(s, UriKind.Absolute)) {
                return new Uri(s);
            }

            throw new ConvertStringFormatException(
                "URL should contain 'http(s)://'");
        }
    }
}