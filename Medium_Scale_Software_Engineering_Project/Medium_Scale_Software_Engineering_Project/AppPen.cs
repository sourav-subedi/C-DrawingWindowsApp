using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Implements the 'Pen' command to set the drawing color on the canvas.
    /// Accepts three parameters (R, G, B) which can be literals or expressions.
    /// RGB values must be integers in the 0–255 range.
    /// </summary>
    public class AppPen : CommandThreeParameters, ICommand
    {
        private ICanvas Canvas;
        private StoredProgram program;
        private string[] parameters;

        /// <summary>
        /// Creates a new AppPen command linked to a specific canvas.
        /// </summary>
        /// <param name="canvas">The canvas where the pen color will be applied.</param>
        public AppPen(ICanvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// Sets the stored program reference and splits the parameters into R, G, B.
        /// </summary>
        /// <param name="Program">Reference to the stored program</param>
        /// <param name="Params">RGB parameters separated by comma or space</param>
        public override void Set(StoredProgram Program, string Params)
        {
            program = Program;
            parameters = Params.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Validates that exactly three parameters are provided for the command.
        /// </summary>
        /// <param name="ParameterList">Parameter array to check (not used)</param>
        /// <exception cref="CommandException">Thrown if parameter count is not exactly 3</exception>
        public override void CheckParameters(string[] ParameterList)
        {
            if (parameters == null || parameters.Length != 3)
                throw new CommandException("Pen command requires exactly 3 parameters for R, G, B.");
        }

        /// <summary>
        /// Compiles the command by verifying parameter count.
        /// </summary>
        /// <exception cref="CommandException">Thrown if parameter count is not exactly 3</exception>
        public override void Compile()
        {
            if (parameters == null || parameters.Length != 3)
                throw new CommandException("Pen command requires exactly 3 parameters for R, G, B.");
        }

        /// <summary>
        /// Executes the Pen command by evaluating each parameter expression
        /// and setting the pen color on the canvas.
        /// </summary>
        /// <exception cref="CommandException">
        /// Thrown if a parameter is invalid, cannot be converted to an integer, 
        /// is out of the 0–255 range, or if setting the color fails.
        /// </exception>
        public override void Execute()
        {
            int red = EvaluateColorParameter(parameters[0], "Red");
            int green = EvaluateColorParameter(parameters[1], "Green");
            int blue = EvaluateColorParameter(parameters[2], "Blue");

            // Validate RGB range
            if (red < 0 || red > 255 || green < 0 || green > 255 || blue < 0 || blue > 255)
                throw new CommandException($"RGB values must be 0–255. Got R={red}, G={green}, B={blue}");

            // Apply color to canvas
            try
            {
                System.Diagnostics.Debug.WriteLine($"AppPen executing: RGB({red},{green},{blue})");
                Canvas.SetColour(red, green, blue);
            }
            catch (Exception ex)
            {
                throw new CommandException($"Failed to set pen color: {ex.Message}");
            }
        }

        /// <summary>
        /// Helper method to evaluate a single color parameter.
        /// </summary>
        /// <param name="param">Parameter string</param>
        /// <param name="colorName">Color name for error messages</param>
        /// <returns>Integer RGB value</returns>
        /// <exception cref="CommandException">Thrown if evaluation fails or value is invalid</exception>
        private int EvaluateColorParameter(string param, string colorName)
        {
            param = param.Trim('<', '>', ' ');

            try
            {
                if (!int.TryParse(param, out int value))
                {
                    string eval = program.EvaluateExpression(param)?.Trim() ?? "";
                    if (!int.TryParse(eval, out value))
                        throw new CommandException($"Invalid {colorName} value: '{eval}'");
                }
                return value;
            }
            catch (Exception ex)
            {
                throw new CommandException($"Invalid {colorName} expression: '{param}' ({ex.Message})");
            }
        }
    }
}
