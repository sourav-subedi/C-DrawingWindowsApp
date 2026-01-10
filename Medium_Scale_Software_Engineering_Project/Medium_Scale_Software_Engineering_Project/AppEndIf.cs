using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Marker command for the end of an if/else block in MYBooseApp.
    /// Works with AppIf and AppElse for control flow.
    /// </summary>
    public class AppEndIf : Command, ICommand
    {
        public AppEndIf() { }

        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);

            string trimmed = (Params ?? "").Trim().ToLower();
            if (trimmed != "" && trimmed != "if")
            {
                throw new CommandException(
                    $"AppEndIf does not accept parameters other than 'if'. Got: '{trimmed}'");
            }
        }

        public override void CheckParameters(string[] parameterList) { }

        public override void Compile() { }

        public override void Execute()
        {
            // Marker command, does nothing.
        }
    }
}

