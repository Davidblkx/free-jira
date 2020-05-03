using System.CommandLine;
using FreeJira.Terminal.Reports.Commands;

namespace FreeJira.Terminal.Reports
{
    internal static class TerminalReportHandler
    {
        public static Command BuildCommand() {
            var cmd = new Command("reports", "Show jira results in predefined format");
            cmd.AddCommand(ReportListCommand.Build());
            return cmd;
        }
    }
}