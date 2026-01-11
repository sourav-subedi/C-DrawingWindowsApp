using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Represents the 'else' branch in an if-else conditional statement.
    /// </summary>
    /// <remarks>
    /// Syntax: "else". This command is executed when the preceding 'if' condition is FALSE.
    /// When executed, it skips over the else block to the matching 'end if' command.
    /// Supports nested if-else statements by tracking nesting levels.
    /// </remarks>
    public class AppElse : Command, ICommand
    {
        /// <summary>
        /// Default constructor for the <see cref="AppElse"/> command.
        /// </summary>
        public AppElse() { }

        /// <summary>
        /// Sets the stored program reference and validates that no parameters are provided.
        /// </summary>
        /// <param name="Program">Reference to the stored program being executed.</param>
        /// <param name="Params">Command parameters (should be empty for 'else').</param>
        /// <exception cref="CommandException">Thrown if any parameters are provided.</exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);

            if (!string.IsNullOrWhiteSpace(Params?.Trim()))
                throw new CommandException("Else command does not accept parameters.");
        }

        /// <summary>
        /// Validates command parameters.
        /// </summary>
        /// <param name="parameterList">Array of parameters to validate.</param>
        /// <remarks>Not used for the 'else' command.</remarks>
        public override void CheckParameters(string[] parameterList) { }

        /// <summary>
        /// Compiles the 'else' command.
        /// </summary>
        /// <remarks>No compilation steps are required for this command.</remarks>
        public override void Compile() { }

        /// <summary>
        /// Executes the 'else' command by skipping to the corresponding 'end if'.
        /// </summary>
        /// <remarks>
        /// Handles nested if-else structures by tracking nesting levels.
        /// </remarks>
        /// <exception cref="BOOSEException">
        /// Thrown if the command is not running under <see cref="AppStoredProgram"/>
        /// or if no matching 'end if' is found.
        /// </exception>
        public override void Execute()
        {
            if (program is not AppStoredProgram extProgram)
                throw new BOOSEException("ElseCommand requires AppStoredProgram.");

            System.Diagnostics.Debug.WriteLine($"Else at PC={program.PC} → skipping else block");

            int nestingLevel = 0;

            for (int i = program.PC + 1; i < extProgram.CommandCount; i++)
            {
                var cmd = extProgram.GetCommand(i);

                if (cmd is AppIf)
                    nestingLevel++;
                else if (cmd is AppEndIf)
                {
                    if (nestingLevel == 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Else jumping to end if at PC={i}");
                        program.PC = i;
                        return;
                    }
                    nestingLevel--;
                }
            }

            throw new BOOSEException("No matching 'end if' found for this 'else'.");
        }
    }
}
