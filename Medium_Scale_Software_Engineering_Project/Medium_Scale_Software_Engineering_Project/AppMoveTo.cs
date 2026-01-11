using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Implements the MoveTo command for moving the pen to specified coordinates.
    /// Supports both literal integers and expressions/variables from the stored program.
    /// </summary>
    public class AppMoveTo : CommandTwoParameters, ICommand
    {
        private ICanvas Canvas;
        private StoredProgram program;
        private string[] parameters;

        /// <summary>
        /// Initializes a new instance with a reference to the canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which to move the pen</param>
        public AppMoveTo(ICanvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// Sets the stored program reference and splits the parameter string.
        /// </summary>
        /// <param name="Program">The stored program instance</param>
        /// <param name="Params">The parameters string, e.g., "100, 200"</param>
        public override void Set(StoredProgram Program, string Params)
        {
            program = Program;
            parameters = Params.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Validates that exactly two parameters are provided.
        /// </summary>
        /// <param name="ParameterList">Array of parameters</param>
        /// <exception cref="CommandException">Thrown if the number of parameters is not two</exception>
        public override void CheckParameters(string[] ParameterList)
        {
            if (parameters == null || parameters.Length != 2)
                throw new CommandException("MoveTo command requires exactly 2 parameters: x y");
        }

        /// <summary>
        /// Compilation phase. MoveTo evaluates expressions at runtime.
        /// Ensures parameter count is correct.
        /// </summary>
        public override void Compile()
        {
            CheckParameters(parameters);
        }

        /// <summary>
        /// Executes the MoveTo command by evaluating X and Y coordinates
        /// and moving the pen to the computed position on the canvas.
        /// </summary>
        /// <exception cref="CommandException">
        /// Thrown when the X or Y parameter cannot be evaluated to a valid integer.
        /// </exception>
        public override void Execute()
        {
            int x, y;

            // Evaluate X coordinate
            string xParam = parameters[0].Trim('<', '>', ' ');
            try
            {
                if (!int.TryParse(xParam, out x))
                {
                    string xEval = program.EvaluateExpression(xParam)?.Trim() ?? "";
                    if (!int.TryParse(xEval, out x))
                        throw new CommandException($"Invalid X value for MoveTo: '{xEval}'");
                }
            }
            catch (Exception ex)
            {
                throw new CommandException(
                    $"Invalid X expression for MoveTo: '{xParam}' ({ex.Message})"
                );
            }

            // Evaluate Y coordinate
            string yParam = parameters[1].Trim('<', '>', ' ');
            try
            {
                if (!int.TryParse(yParam, out y))
                {
                    string yEval = program.EvaluateExpression(yParam)?.Trim() ?? "";
                    if (!int.TryParse(yEval, out y))
                        throw new CommandException($"Invalid Y value for MoveTo: '{yEval}'");
                }
            }
            catch (Exception ex)
            {
                throw new CommandException(
                    $"Invalid Y expression for MoveTo: '{yParam}' ({ex.Message})"
                );
            }

            // Move pen on the canvas
            Canvas.MoveTo(x, y);
        }
    }
}
