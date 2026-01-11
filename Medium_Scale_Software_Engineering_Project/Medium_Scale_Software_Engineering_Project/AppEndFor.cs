using BOOSE;
using System.Diagnostics;

namespace MYBooseApp
{
    /// <summary>
    /// Represents the end of a for loop and manages loop iteration.
    /// </summary>
    /// <remarks>
    /// Syntax: "end for" or "endfor". 
    /// Increments the loop variable and returns execution to the corresponding for statement
    /// to evaluate the loop condition and continue iteration.
    /// </remarks>
    public class AppEndFor : Command, ICommand
    {
        /// <summary>
        /// Reference to the matching <see cref="AppFor"/> command.
        /// </summary>
        private AppFor correspondingFor;

        /// <summary>
        /// Default constructor required for instantiation via <see cref="AppCommandFactory"/>.
        /// </summary>
        public AppEndFor() { }

        /// <summary>
        /// Sets the stored program reference and validates parameters.
        /// Only accepts an empty string or "for" (split from "end for").
        /// </summary>
        /// <param name="Program">Reference to the stored program being executed.</param>
        /// <param name="Params">Command parameters (should be empty or "for").</param>
        /// <exception cref="CommandException">Thrown if invalid parameters are provided.</exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);

            string trimmed = (Params ?? "").Trim().ToLower();
            if (trimmed != "" && trimmed != "for")
            {
                throw new CommandException(
                    $"End for command only accepts 'for' or no parameters. Got: '{trimmed}'");
            }
        }

        /// <summary>
        /// Validates command parameters.
        /// </summary>
        /// <param name="parameterList">Array of parameters (not used).</param>
        /// <remarks>No action required for the 'end for' command.</remarks>
        public override void CheckParameters(string[] parameterList) { }

        /// <summary>
        /// Links this EndFor command to its corresponding <see cref="AppFor"/> during compilation.
        /// Pops the AppFor from the program stack and stores the link.
        /// </summary>
        /// <exception cref="CommandException">Thrown if the popped command is not a valid <see cref="AppFor"/>.</exception>
        public override void Compile()
        {
            var forCmd = program.Pop();
            if (forCmd is not AppFor)
                throw new CommandException("End for does not match a for statement.");

            correspondingFor = (AppFor)forCmd;
            int myIndex = program.Count - 1;
            correspondingFor.SetEndForIndex(myIndex);

            Debug.WriteLine($"EndFor at index {myIndex} linked to For at index {correspondingFor.GetForIndex()}");
        }

        /// <summary>
        /// Executes the end for command by incrementing the loop variable
        /// and jumping back to the corresponding for statement for iteration.
        /// </summary>
        /// <exception cref="BOOSEException">Thrown if the command was not properly compiled.</exception>
        public override void Execute()
        {
            if (correspondingFor == null)
                throw new BOOSEException("EndFor not properly compiled.");

            string varName = GetLoopVariableName();
            int currentValue = int.Parse(program.GetVarValue(varName));
            int incrementValue = GetIncrementValue();
            int newValue = currentValue + incrementValue;

            program.UpdateVariable(varName, newValue);

            Debug.WriteLine($"EndFor: Incremented {varName} from {currentValue} to {newValue}");

            int forIndex = correspondingFor.GetForIndex();
            Debug.WriteLine($"EndFor at PC={program.PC - 1} → jumping back to for at PC={forIndex}");

            program.PC = forIndex;
        }

        /// <summary>
        /// Retrieves the loop variable name from the corresponding <see cref="AppFor"/>.
        /// </summary>
        /// <returns>The loop variable name.</returns>
        private string GetLoopVariableName() => correspondingFor.GetVarName();

        /// <summary>
        /// Retrieves the increment value from the corresponding <see cref="AppFor"/>.
        /// </summary>
        /// <returns>The increment value.</returns>
        private int GetIncrementValue() => correspondingFor.GetIncrement();
    }
}
