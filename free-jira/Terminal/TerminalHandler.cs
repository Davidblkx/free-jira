using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using FreeJira.Terminal.Profiles;

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

            var rootCommand = new RootCommand() {
                ProfilesCommand.BuildProfileCommand(),
            };

            return await rootCommand.InvokeAsync(args);
        }
    }
}