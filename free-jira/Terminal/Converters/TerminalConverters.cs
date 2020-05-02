using System;
using ConsoleInteractive.InputConverter;

namespace FreeJira.Terminal.Converters
{
    /// <summary>
    /// Register converters
    /// </summary>
    internal static class TerminalConverters
    {
        public static void Register()
        {
            StringConverterProvider.Global.Register(new UriConverter(), true);
            StringConverterProvider.Global.Register(new DateConverter(), true);
        }
    }
}