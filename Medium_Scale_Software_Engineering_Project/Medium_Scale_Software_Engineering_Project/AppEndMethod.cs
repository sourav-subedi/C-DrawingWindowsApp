using BOOSE;
using System.Diagnostics;

namespace MYBooseApp
{
    /// <summary>
    /// Represents the end of a method declaration in the BOOSE language.
    /// Serves as a marker indicating the method body has finished.
    /// When executed, it removes local parameters and returns control to the calling instruction.
    /// Syntax: "end method" or "end".
    /// </summary>
    public class AppEndMethod : Command, ICommand
    {
        /// <summary>
        /// Reference to the corresponding AppMethod command for this EndMethod.
        /// </summary>
        private AppMethod correspondingMethod;

        /// <summary>
        /// The program counter (PC) address to return to after method execution.
        /// </summary>
        private int returnAddress = -1;

        /// <summary>
        /// Default constructor required for instantiation via <see cref="AppCommandFactory"/>.
        /// </summary>
        public AppEndMethod() { }

        /// <summary>
        /// Sets the stored program reference and validates parameters.
        /// Accepts an empty string or "method" (when parser splits "end method" into "end" + "method").
        /// </summary>
        /// <param name="Program">Reference to the stored program being executed.</param>
        /// <param name="Params">Command parameters (should be empty or "method").</param>
        /// <exception cref="CommandException">Thrown if invalid parameters are provided.</exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);

            string trimmed = (Params ?? "").Trim().ToLower();
            if (trimmed != "" && trimmed != "method")
            {
                throw new CommandException(
                    $"End method command only accepts 'method' or no parameters. Got: '{trimmed}'");
            }
        }

        /// <summary>
        /// Parameter validation (not required for AppEndMethod).
        /// </summary>
        /// <param name="parameterList">Array of parameters (not used).</param>
        public override void CheckParameters(string[] parameterList) { }

        /// <summary>
        /// Compilation step: Links this EndMethod with its corresponding AppMethod.
        /// Pops the method from the BOOSE compilation stack and stores the reference.
        /// </summary>
        /// <exception cref="CommandException">Thrown if no matching method is found or type mismatch occurs.</exception>
        public override void Compile()
        {
            var poppedCommand = program.Pop();

            if (poppedCommand is not ConditionalCommand)
            {
                throw new CommandException(
                    "End method does not match a method declaration. Ensure AppMethod extends ConditionalCommand or CompoundCommand.");
            }

            correspondingMethod = poppedCommand as AppMethod;
            if (correspondingMethod == null)
                throw new CommandException("Popped command is not a valid AppMethod.");

            int myIndex = program.Count - 1;
            correspondingMethod.SetEndMethodIndex(myIndex);

            Debug.WriteLine($"EndMethod at index {myIndex} linked to Method '{correspondingMethod.GetMethodName()}' at index {correspondingMethod.GetMethodStartIndex()}");
        }

        /// <summary>
        /// Sets the program counter address to return to after method execution.
        /// Called by <see cref="AppCall"/> when invoking the method.
        /// </summary>
        /// <param name="address">Program counter (PC) of the instruction after the call.</param>
        public void SetReturnAddress(int address)
        {
            returnAddress = address;
        }

        /// <summary>
        /// Executes the EndMethod command.
        /// Cleans up method parameters (local variables) and returns control to the calling instruction.
        /// </summary>
        /// <exception cref="BOOSEException">Thrown if EndMethod is executed without a return address.</exception>
        public override void Execute()
        {
            if (returnAddress == -1)
                throw new BOOSEException("EndMethod executed without a return address (not called via AppCall).");

            Debug.WriteLine($"EndMethod at PC={program.PC - 1} → returning to PC={returnAddress}");

            var parameters = correspondingMethod.GetParameters();
            foreach (var (type, name) in parameters)
            {
                if (program.VariableExists(name))
                    program.DeleteVariable(name);
            }

            program.PC = returnAddress;
            returnAddress = -1;
        }
    }
}
