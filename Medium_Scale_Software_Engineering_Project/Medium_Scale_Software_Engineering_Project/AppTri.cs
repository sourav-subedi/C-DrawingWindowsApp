using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Command class for drawing a triangle on the canvas.
    /// Evaluates width and height at runtime, allowing the use of
    /// literal integers or expressions involving BOOSE variables.
    /// </summary>
    public class AppTri : CommandTwoParameters, ICommand
    {
        /// <summary>
        /// Parameters provided to the triangle command.
        /// </summary>
        private string[] Parameters;

        /// <summary>
        /// Reference to the canvas on which the triangle is drawn.
        /// </summary>
        private ICanvas Canvas;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppTri"/> class.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        public AppTri(ICanvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// Sets the stored program reference and splits the command parameters.
        /// </summary>
        /// <param name="Program">The current <see cref="StoredProgram"/> instance.</param>
        /// <param name="Params">The parameter string (e.g., "50 80").</param>
        public override void Set(StoredProgram Program, string Params)
        {
            program = Program;
            Parameters = Params.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Validates that exactly two parameters (width and height) are provided.
        /// </summary>
        /// <param name="Parameters">The array of parameters.</param>
        /// <exception cref="CommandException">Thrown if the parameter count is not exactly two.</exception>
        public override void CheckParameters(string[] Parameters)
        {
            if (Parameters.Length != 2)
            {
                throw new CommandException(
                    "Triangle command requires 2 parameters: width height. Example: 'triangle 50 80'."
                );
            }
        }

        /// <summary>
        /// Compiles the triangle command.
        /// Runtime evaluation of width and height occurs in <see cref="Execute"/>.
        /// </summary>
        /// <exception cref="CommandException">Thrown if parameters are missing or invalid.</exception>
        public override void Compile()
        {
            if (Parameters == null || Parameters.Length != 2)
            {
                throw new CommandException(
                    "Triangle command requires 2 parameters: width height. Example: 'triangle 50 80'."
                );
            }
        }

        /// <summary>
        /// Executes the triangle drawing command.
        /// Evaluates width and height expressions and draws the triangle on the canvas.
        /// </summary>
        /// <exception cref="BOOSEException">
        /// Thrown if width or height is invalid or non-positive.
        /// </exception>
        public override void Execute()
        {
            int width, height;

            // Evaluate width
            string widthParam = Parameters[0].Trim('<', '>', ' ');
            if (!int.TryParse(widthParam, out width))
            {
                string evalResult = program.EvaluateExpression(widthParam)?.Trim() ?? "";
                if (!int.TryParse(evalResult, out width))
                {
                    throw new BOOSEException(
                        $"Invalid width value for Triangle: '{widthParam}' (evaluated as '{evalResult}')"
                    );
                }
            }

            // Evaluate height
            string heightParam = Parameters[1].Trim('<', '>', ' ');
            if (!int.TryParse(heightParam, out height))
            {
                string evalResult = program.EvaluateExpression(heightParam)?.Trim() ?? "";
                if (!int.TryParse(evalResult, out height))
                {
                    throw new BOOSEException(
                        $"Invalid height value for Triangle: '{heightParam}' (evaluated as '{evalResult}')"
                    );
                }
            }

            // Validate dimensions
            if (width <= 0 || height <= 0)
            {
                throw new BOOSEException(
                    $"Triangle width and height must be positive integers. Got width={width}, height={height}"
                );
            }

            // Draw triangle on canvas
            Canvas.Tri(width, height);
        }
    }
}
