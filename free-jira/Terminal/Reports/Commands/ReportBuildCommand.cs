using System.Collections.Generic;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using FreeJira.Helpers;
using FreeJira.Jira.Profile.Sprint;

namespace FreeJira.Terminal.Reports.Commands
{
    internal static class ReportBuildCommand
    {
        public static Command Build() {
            var cmd = new Command("build", "Show what params are available for a report");
            cmd.AddOption(new Option<string>("--report-name"));
            cmd.AddOption(new Option<string>("--sprint", "Sprint name, use active sprint by default"));
            cmd.AddOption(new Option<string>("--profile", "Profile to use"));
            cmd.AddOption(BuildParamOption());
            cmd.Handler = CommandHandler.Create<string, IEnumerable<string>, string, string>(BuildReport);
            return cmd;
        }

        private static Option<string> BuildParamOption() {
            var names = new string[] { "--param", "-p" };
            var opt = new Option<string>(names, "Parameters of report");
            opt.AddValidator(symbol => 
                symbol.Tokens
                    .Select(e => e.Value.Split(':'))
                    .Where(e => ValidateParam(e))
                    .Select(_ => "Params should be in format [key]:[value]")
                    .FirstOrDefault()
            );
            opt.Argument = new Argument<string> { Arity = ArgumentArity.OneOrMore };
            return opt;
        }

        private static bool ValidateParam(string[] param) {
            return param.Length == 2 &&
                !string.IsNullOrEmpty(param[0]) &&
                !string.IsNullOrEmpty(param[1]);
        }

        private static async Task BuildReport(
            string? reportName,
            IEnumerable<string>? param,
            string? sprint,
            string? profile
        ) {
            var report = await TerminalReportHandler
                .GetReportByName(reportName);

            if (report is null) {
                Console.WriteLine("Can´t find report " + reportName ?? "");
                return;
            }
            
            var paramsData = BuildParams(param);
            var jiraSprint = await GetSprint(profile, sprint);
            var client = await ProfileHelpers.GetJiraClient(profile);

            if (client is null) {
                Console.WriteLine("Can't load jira service, check default profile is valid");
                return;
            }

            await report.PrintReport(client, jiraSprint, paramsData);
        }

        private static Dictionary<string, string> BuildParams(
            IEnumerable<string>? param
        ) {
            if (param is null) return new Dictionary<string, string>();
            var dictionary = new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);
            return param.Select(e => TryParseParam(e))
                .Where(e => !(e is null))
                .Cast<KeyValuePair<string, string>>()
                .Aggregate(dictionary, AddToDictionary);
        }

        private static KeyValuePair<string, string>? TryParseParam(string? p) {
            var data = p?.Split(':');
            if (data is null || !ValidateParam(data)) return null;
            return new KeyValuePair<string, string>(data[0], data[1]);
        }

        private static Dictionary<string, string> AddToDictionary(
            Dictionary<string, string> d,
            KeyValuePair<string, string> p
        ) {
            d.Add(p.Key, p.Value);
            return d;
        }

        private static async Task<IJiraSprint?> GetSprint(string? profile, string? sprint) {
            var sprintService = await ProfileHelpers.GetJiraSprintService(profile);
            return sprintService?.GetSprintByName(sprint ?? "")
                ?? sprintService?.GetActiveSprint();
        }
    }
}