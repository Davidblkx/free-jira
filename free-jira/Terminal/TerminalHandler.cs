using System.Linq;

namespace free_jira.Terminal
{
    /// <summary>
    /// Handle arguments for console interation,
    /// If --server is passed, start HTTP server
    /// </summary>
    public static class TerminalHandler
    {
        public static bool HandleArgs(string[] args) {
            if (args.Count(e => e == "--server") == 1) {
                return false;
            }

            return true;
        }
    }
}