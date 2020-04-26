using System;
using ConsoleInteractive.Components;

namespace FreeJira.Terminal.Components
{
    public static class TerminalComponents {
        public const string UriInput = "COMPONENT_URI";
    }

    public static class TerminalComponentsImpl
    {
        public static void Register() {
            ComponentsProvider.Global.Register(
                TerminalComponents.UriInput, InputText.Create<Uri>("Insert URL"));
        }
    }
}