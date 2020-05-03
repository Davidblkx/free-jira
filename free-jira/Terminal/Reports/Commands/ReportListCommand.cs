using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using FreeJira.Jira.ReportEngine;

namespace FreeJira.Terminal.Reports.Commands
{
    internal static class ReportListCommand
    {
        public static Command Build(){
            var cmd = new Command("list", "List all available reports");
            cmd.AddOption(new Option<bool>("--print-params", () => true, "Print report params"));
            cmd.Handler = CommandHandler.Create<bool>(ListReports);
            return cmd;
        }

        private static void ListReports(bool printParams) {

            ReportEngineService.GetReports()
                .ToList().ForEach(e => PrintReport(e, printParams));
        }

        private static void PrintReport(IJiraReport? r, bool printParams) {
            PrintLine();
            Console.WriteLine($"{r?.Name}: {r?.Description}");
            if (printParams && !(r is null)) {
                foreach(var p in r.ParamsMap)
                    Console.WriteLine($"    {p.Key}: {p.Value}");
            }
            PrintLine();
        }

        private static void PrintLine() {
            for(var i = 0; i < 80; i++)
                Console.Write("-");
            Console.Write("\n");
        }
    }
}