using ConsoleInteractive.InputValidation;

namespace FreeJira.Terminal.Validators
{
    public static class StringValidators
    {
        public const string REQUIRED = "VAL_STRING_REQUIRED";
    }

    internal static class StringValidatorsImpl
    {
        /// <summary>
        /// Register string validators
        /// </summary>
        public static void Register() {
            ValidatorProvider.Global.Register(
                StringValidators.REQUIRED, CreateRequiredValidator(), true);
        }

        private static IValidatorCollection<string> CreateRequiredValidator()
            => ValidatorCollection.Create<string>()
                .Add(e => (!string.IsNullOrEmpty(e), "Value can't be empty"));
    }
}