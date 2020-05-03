using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using System.Threading.Tasks;
using ConsoleInteractive;
using FreeJira.Jira.Profile;
using FreeJira.Terminal.Components;
using FreeJira.Terminal.Converters;
using FreeJira.Terminal.Jql;
using FreeJira.Terminal.Profiles;
using FreeJira.Terminal.Reports;
using FreeJira.Terminal.Sprints;
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
                TerminalSprintService.BuildSprintCommand(),
                TerminalJqlService.BuildCommand(),
                TerminalReportHandler.BuildCommand()
            };

            rootCommand.AddOption(
                new Option("--server", "run in server mode"));

            return await new CommandLineBuilder(rootCommand)
                .UseMiddleware(CheckProfilesCount)
                .UseHelp()
                .Build().InvokeAsync(args);
        }

        private static void InitTerminal() {
            ConsoleI.RegisterDefaults();
            TerminalValidators.Register();
            TerminalConverters.Register();
            TerminalComponentsImpl.Register();
        }

        /// <summary>
        /// Stop execution if no profile is defined
        /// </summary>
        /// <param name="context"></param>
        /// <param name="Func<InvocationContext"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private static async Task CheckProfilesCount(
            InvocationContext context,
            Func<InvocationContext, Task> next
        ) {
            var isProfileCommand = CheckIsProfileCommand(context);
            if (isProfileCommand || await CheckHasProfiles()) {
                await next(context);
                return;
            }
            
            Console.WriteLine("No Jira Profile is defined, please create at least 1 profile");
        }

        private static bool CheckIsProfileCommand(InvocationContext context) {
            return context.ParseResult.Tokens
                .Any(e => e.Type == TokenType.Command && e.Value == "profile");
        }

        private static async Task<bool> CheckHasProfiles() {
            return (await JiraProfileService.GetAvailableProfiles())
                .Count() > 0;
        }
    }
}