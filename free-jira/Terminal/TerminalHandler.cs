using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using ConsoleInteractive;
using FreeJira.Terminal.Components;
using FreeJira.Terminal.Converters;
using FreeJira.Terminal.Profiles;
using FreeJira.Terminal.Validators;

namespace FreeJira.Terminal
{
    /// <summary>
    /// Handle arguments for console interation,
    /// If --server is passed, start HTTP server
    /// </summary>
    public static class TerminalHandler
    {
        public static async Task<int> HandleArgs(string[] args) {
            if (args.Count(e => e == "--server") == 1) {
                return 0;
            }

            InitTerminal();
            var rootCommand = new RootCommand() {
                TerminalProfileService.BuildProfileCommand(),
            };
            rootCommand.AddGlobalOption(
                new Option("--terminal", "run in terminal mode"));
            rootCommand.AddGlobalOption(
                new Option("--server", "run in server mode"));

            return await rootCommand.InvokeAsync(args);
        }

        private static void InitTerminal() {
            ConsoleI.RegisterDefaults();
            TerminalValidators.Register();
            TerminalConverters.Register();
            TerminalComponentsImpl.Register();
        }
    }
}