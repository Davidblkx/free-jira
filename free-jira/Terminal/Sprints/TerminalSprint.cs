using System.CommandLine;
using FreeJira.Terminal.Sprints.Commands;

namespace FreeJira.Terminal.Sprints
{
    public class TerminalSprintService
    {
        public static Command BuildSprintCommand() {
            var cmd = new Command("sprint", "Manage sprints");
            
            var optionProfile = new Option<string>(
                new string[] {"--profile", "-p"}, "Profile to use");
            cmd.AddOption(optionProfile);

            cmd.AddCommand(SprintCreateCommand.Build());
            cmd.AddCommand(SprintListCommand.Build());

            return cmd;
        }
    }
}