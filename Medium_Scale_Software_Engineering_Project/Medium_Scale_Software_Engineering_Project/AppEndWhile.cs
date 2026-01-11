using BOOSE;
using System.Diagnostics;

namespace MYBooseApp
{
    /// <summary>
    /// Represents the end of a while loop in the BOOSE language.
    /// Serves as a marker to return control to the corresponding <see cref="AppWhile"/> for condition evaluation.
    /// Syntax: "end while" or "endwhile".
    /// Uses a forward-scanning mechanism—no backward linking is required.
    /// </summary>
    public class AppEndWhile : Command, ICommand
    {
        /// <summary>
        /// Reference to the corresponding <see cref="AppWhile"/> command.
        /// Established during compilation.
        /// </summary>
        private AppWhile correspondingWhile;

        /// <summary>
        /// Default constructor required for instantiation via <see cref="AppCommandFactory"/>.
        /// </summary>
        public AppEndWhile() { }

        /// <summary>
        /// Sets the stored program reference and validates parameters.
        /// Only allows an empty string or "while" (when parser splits "end while").
        /// </summary>
        /// <param name="Program">Reference to the stored program being executed.</param>
        /// <param name="Params">Optional parameter, should be empty or "while".</param>
        /// <exception cref="CommandException">Thrown if invalid parameters are provided.</exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);
            string trimmed = (Params ?? "").Trim().ToLower();

            if (trimmed != "" && trimmed != "while")
            {
                throw new CommandException(
                    $"End while command only accepts 'while' or no parameters. Got: '{trimmed}'");
            }
        }

        /// <summary>
        /// Parameter validation is not required for EndWhile.
        /// </summary>
        /// <param name="parameterList">Array of parameters (not used).</param>
        public override void CheckParameters(string[] parameterList) { }

        /// <summary>
        /// Compilation phase: Pops the corresponding <see cref="AppWhile"/> from the stack
        /// and stores a reference for execution.
        /// </summary>
        /// <exception cref="CommandException">Thrown if no matching while statement is found.</exception>
        public override void Compile()
        {
            var whileCmd = program.Pop();

            if (whileCmd is not AppWhile)
            {
                throw new CommandException("End while does not match a while statement.");
            }

            correspondingWhile = (AppWhile)whileCmd;

            Debug.WriteLine($"EndWhile at index {program.Count - 1} linked to While at index {correspondingWhile.GetWhileIndex()}");
        }

        /// <summary>
        /// Execution phase: Sets the program counter (PC) to the corresponding <see cref="AppWhile"/>
        /// so the loop condition can be re-evaluated.
        /// </summary>
        /// <exception cref="BOOSEException">Thrown if EndWhile was not properly compiled or linked.</exception>
        public override void Execute()
        {
            if (correspondingWhile == null)
                throw new BOOSEException("EndWhile not properly compiled.");

            int whileIndex = correspondingWhile.GetWhileIndex();

            Debug.WriteLine($"EndWhile at PC={program.PC - 1} → jumping back to while at PC={whileIndex}");

            program.PC = whileIndex;
        }
    }
}
