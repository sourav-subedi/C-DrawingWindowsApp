using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Represents the end of an if/else block in the BOOSE language.
    /// Serves as a marker to indicate where an if/else statement finishes.
    /// Execution of this command has no effect; it is only used for control flow.
    /// </summary>
    public class AppEndIf : Command, ICommand
    {
        /// <summary>
        /// Default constructor required for instantiation via <see cref="AppCommandFactory"/>.
        /// </summary>
        public AppEndIf() { }

        /// <summary>
        /// Sets the stored program reference and validates parameters.
        /// Accepts an empty string or "if" (when parser splits "end if" into "end" + "if").
        /// </summary>
        /// <param name="Program">Reference to the stored program being executed.</param>
        /// <param name="Params">Command parameters (should be empty or "if").</param>
        /// <exception cref="CommandException">Thrown if invalid parameters are provided.</exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);

            string trimmed = (Params ?? "").Trim().ToLower();
            if (trimmed != "" && trimmed != "if")
            {
                throw new CommandException(
                    $"End if command only accepts 'if' or no parameters. Got: '{trimmed}'");
            }
        }

        /// <summary>
        /// Parameter validation (not required for AppEndIf).
        /// </summary>
        /// <param name="parameterList">Array of parameters (not used).</param>
        public override void CheckParameters(string[] parameterList) { }

        /// <summary>
        /// Compilation step (not required for AppEndIf).
        /// </summary>
        public override void Compile() { }

        /// <summary>
        /// Execution step (not required for AppEndIf).
        /// This command is used as a marker for control flow only.
        /// </summary>
        public override void Execute() { }
    }
}
