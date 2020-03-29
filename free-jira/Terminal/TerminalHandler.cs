using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using free_jira.Terminal.Profiles;

namespace free_jira.Terminal
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