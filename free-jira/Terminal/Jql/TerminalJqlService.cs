using System.Linq;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using FreeJira.Helpers;
using FreeJira.Jira.Model;
using System.Text.Json;

namespace FreeJira.Terminal.Jql
{
    internal static class TerminalJqlService
    {
        public static Command BuildCommand() {
            var cmd = new Command("jql", "Execute JQL and print results");
            cmd.AddOption(new Option<string>(new string[] {"--profile", "-p"}, "Profile to use"));
            cmd.AddOption(new Option<string>(new string[] {"--fields", "-f"}, "Extras fields to get"));
            cmd.AddOption(new Option<bool>("--export", "Export to csv"));
            cmd.AddArgument(new Argument<string>("jql"));
            cmd.Handler = CommandHandler.Create<string, string>(ExecuteJql);
            return cmd;
        }

        private static async Task ExecuteJql(string jql, string? profile) {
            var client = await ProfileHelpers.GetJiraClient(profile);
            if (client is null) { Console.WriteLine("Profile not found!"); return; }

            var res = await client.IssueClient.Jql<object>(jql, new { Summary = ""});
            
            res.Match(s => Handle(s), () => Console.WriteLine("JQL didn't return results"));
        }

        private static void Handle<T>(JiraIssueJqlResponse<T> response) where T : class {
            response.Issues
                .ToList()
                .ForEach(e => Console.WriteLine($"{e.Key}: {e.Self} [{GetString("summary", e.Fields)}]"));
        }

        private static string GetString(string property, object? field) {
            if (field is JsonElement elem)
                elem.GetProperty(property).GetString();
            return "";
        }

    }
}