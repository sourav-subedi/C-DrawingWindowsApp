using BOOSE;
using System.Collections.Generic;

namespace MYBooseApp
{
    /// <summary>
    /// Extended version of <see cref="StoredProgram"/> with support for
    /// custom method registration and enhanced runtime execution.
    /// </summary>
    public class AppStoredProgram : StoredProgram
    {
        /// <summary>
        /// Registry of method names to their start indices in the command list.
        /// </summary>
        private Dictionary<string, int> methodRegistry = new Dictionary<string, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppStoredProgram"/> class
        /// with the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which commands will operate.</param>
        public AppStoredProgram(ICanvas canvas) : base(canvas) { }

        /// <summary>
        /// Gets the total number of commands in the program.
        /// </summary>
        public int CommandCount => Count;

        /// <summary>
        /// Registers a custom method by name and the index of its first command.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="startIndex">The index of the first command of the method.</param>
        public void RegisterMethod(string methodName, int startIndex)
        {
            methodRegistry[methodName] = startIndex;
        }

        /// <summary>
        /// Retrieves the start index of a registered method.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <returns>The start index of the method.</returns>
        /// <exception cref="BOOSEException">Thrown if the method does not exist.</exception>
        public int GetMethodStartIndex(string methodName)
        {
            if (!methodRegistry.ContainsKey(methodName))
                throw new BOOSEException($"No such method: {methodName}");

            return methodRegistry[methodName];
        }

        /// <summary>
        /// Determines whether a method with the specified name exists.
        /// </summary>
        /// <param name="methodName">The method name to check.</param>
        /// <returns>True if the method exists; otherwise, false.</returns>
        public bool MethodExists(string methodName)
        {
            return methodRegistry.ContainsKey(methodName);
        }

        /// <summary>
        /// Retrieves the command at the specified index.
        /// </summary>
        /// <param name="index">The command index.</param>
        /// <returns>The command at the given index.</returns>
        /// <exception cref="BOOSEException">
        /// Thrown if the index is out of range or the item is not an <see cref="ICommand"/>.
        /// </exception>
        public ICommand GetCommand(int index)
        {
            if (index < 0 || index >= Count)
                throw new BOOSEException($"Command index out of range: {index} (total commands: {Count})");

            if (this[index] is not ICommand cmd)
                throw new BOOSEException($"Item at index {index} is not a valid ICommand");

            return cmd;
        }

        /// <summary>
        /// Executes the stored program, iterating through all commands.
        /// Detects potential infinite loops and reports syntax or runtime errors.
        /// </summary>
        /// <exception cref="StoredProgramException">
        /// Thrown if the program contains syntax errors or a runtime exception occurs.
        /// </exception>
        public override void Run()
        {
            string errors = "";
            int iterations = 0;
            const int MAX_ITERATIONS = 10000; // Safety limit for large loops

            if (!IsValidProgram())
            {
                throw new StoredProgramException("Cannot run program with syntax errors.");
            }

            while (Commandsleft())
            {
                ICommand command = (ICommand)NextCommand();

                try
                {
                    iterations++;

                    if (iterations > MAX_ITERATIONS)
                    {
                        throw new StoredProgramException($"Possible infinite loop detected after {iterations} iterations.");
                    }

                    command.Execute();
                }
                catch (BOOSEException ex)
                {
                    SetSyntaxStatus(false);
                    errors += $"Error: {ex.Message} at PC={PC}\n";
                    break;
                }
            }

            if (!IsValidProgram())
            {
                throw new StoredProgramException(errors.Trim());
            }
        }
    }
}
