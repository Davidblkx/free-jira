using System.CommandLine;
using FreeJira.Terminal.Profiles.Commands;

namespace FreeJira.Terminal.Profiles
{
    public static class TerminalProfileService
    {
        public static Command BuildProfileCommand() {
            var cmd = new Command("profile") { Description = "Profile related actions" };
            
            cmd.AddCommand(ProfileList.GetListCommand());
            cmd.AddCommand(ProfilesCreate.GetCreateProfileCommand());
            cmd.AddCommand(ProfilesDelete.GetDeleteProfileCommand());
            cmd.AddCommand(ProfileDefault.GetSetDefaultCommand());
            cmd.AddCommand(ProfileDefault.GetGetDefaultCommand());

            return cmd;
        }
    }
}