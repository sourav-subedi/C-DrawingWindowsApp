using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Represents the Reset command for the BOOSE language.
    /// Resets the canvas state to its default settings.
    /// </summary>
    public class AppReset : CommandOneParameter, ICommand
    {
        private string[] Parameters;   // Parameters array (unused)
        private ICanvas Canvas;        // Reference to the canvas
        private StoredProgram program; // Reference to the stored program

        /// <summary>
        /// Initializes a new instance of the <see cref="AppReset"/> class
        /// with the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas to reset.</param>
        public AppReset(ICanvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// Sets the stored program reference and initializes parameters.
        /// Reset does not require any parameters.
        /// </summary>
        /// <param name="Program">The stored program associated with this command.</param>
        /// <param name="Params">Command parameters (ignored).</param>
        public override void Set(StoredProgram Program, string Params)
        {
            program = Program;
            Parameters = new string[0]; // Reset has no parameters
        }

        /// <summary>
        /// Validates parameters for the Reset command.
        /// Ensures that no extra parameters are provided.
        /// </summary>
        /// <param name="Parameters">Array of parameters to validate.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if any parameters are provided to the Reset command.
        /// </exception>
        public override void CheckParameters(string[] Parameters)
        {
            if (Parameters.Length > 0)
            {
                throw new ArgumentException(
                    "Reset command does not accept any parameters. Example: 'reset'"
                );
            }
        }

        /// <summary>
        /// Compiles the Reset command.
        /// No compilation logic is required for this command.
        /// </summary>
        public override void Compile()
        {
            // No compilation required for Reset
        }

        /// <summary>
        /// Executes the Reset command by resetting the canvas to its default state.
        /// </summary>
        public override void Execute()
        {
            Canvas.Reset();
        }
    }
}
