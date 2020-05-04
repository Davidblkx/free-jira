using System.Linq;
using System.Threading.Tasks;
using System.CommandLine;
using FreeJira.Terminal.Reports.Commands;
using FreeJira.Jira.ReportEngine;
using ConsoleInteractive;

namespace FreeJira.Terminal.Reports
{
    internal static class TerminalReportHandler
    {
        public static Command BuildCommand() {
            var cmd = new Command("reports", "Show jira results in predefined format");
            cmd.AddCommand(ReportListCommand.Build());
            cmd.AddCommand(ReportBuildCommand.Build());
            return cmd;
        }

        public static async Task<IJiraReport?> GetReportByName(string? reportName) {
            var availableReports = ReportEngineService.GetReports();
            return string.IsNullOrEmpty(reportName) ?
                await ConsoleI.Select(availableReports) :
                availableReports.FirstOrDefault(e => e?.Name == reportName);
        }
    }
}