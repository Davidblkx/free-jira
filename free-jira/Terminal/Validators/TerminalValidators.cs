namespace FreeJira.Terminal.Validators
{
    /// <summary>
    /// Register validators
    /// </summary>
    internal static class TerminalValidators
    {
        public static void Register() {
            StringValidatorsImpl.Register();
        }
    }
}