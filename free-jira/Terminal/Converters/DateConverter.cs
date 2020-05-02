using System.Net.Http.Headers;
using System;
using ConsoleInteractive.InputConverter;

namespace FreeJira.Terminal.Converters
{
    public class DateConverter : StringConverter<DateTime?>
    {
        public DateConverter(): base(DateToString, StringToDate) {}

        public static string DateToString(DateTime? u) => u?.ToString("YYYY-MM-DD") ?? "";
        public static DateTime? StringToDate(string s) {
            if (DateTime.TryParse(s, out DateTime d))
                return d;

            throw new ConvertStringFormatException(
                "Date should be in format YYYY-MM-DD");
        }
    }
}