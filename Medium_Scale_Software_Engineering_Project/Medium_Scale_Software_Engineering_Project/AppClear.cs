using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Implements the Clear command for the BOOSE language.
    /// Clears the entire canvas and removes all drawn content.
    /// </summary>
    public class AppClear : CommandOneParameter, ICommand
    {
        string[] Parameters;       // Parameters array
        private ICanvas Canvas;    // Reference to canvas

        /// <summary>
        /// Creates a new AppClear command using the specified canvas.
        /// </summary>
        /// <param name="canvas">Canvas to be cleared when the command executes.</param>
        public AppClear(ICanvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// Assigns the StoredProgram reference for the command.
        /// The Clear command does not accept any parameters.
        /// </summary>
        /// <param name="Program">The current stored program.</param>
        /// <param name="Params">Unused parameter string.</param>
        public override void Set(StoredProgram Program, string Params)
        {
            program = Program;
            Parameters = new string[0];
        }

        /// <summary>
        /// Validates that no parameters are supplied to the Clear command.
        /// </summary>
        /// <param name="Parameters">Array of parsed parameters.</param>
        public override void CheckParameters(string[] Parameters)
        {
            if (Parameters.Length > 0)
            {
                throw new ArgumentException(
                    "Clear command does not accept any parameters. Example: 'clear'"
                );
            }
        }

        /// <summary>
        /// Compiles the Clear command.
        /// No compilation logic is required.
        /// </summary>
        public override void Compile()
        {
            // No compilation required
        }

        /// <summary>
        /// Executes the Clear command by clearing the canvas.
        /// </summary>
        public override void Execute()
        {
            Canvas.Clear();
        }
    }
}
