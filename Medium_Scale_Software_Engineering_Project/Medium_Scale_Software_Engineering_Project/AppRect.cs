using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a rectangle drawing command in the BOOSE language.
    /// Draws a rectangle on the canvas using width and height expressions,
    /// and optionally supports a filled flag.
    /// </summary>
    public class AppRect : CommandThreeParameters, ICommand
    {
        private ICanvas Canvas;
        private StoredProgram program;
        private string[] Parameters;
        private bool filled = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppRect"/> class
        /// with a reference to the application canvas.
        /// </summary>
        /// <param name="canvas">The canvas used for rendering the rectangle.</param>
        public AppRect(ICanvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// Sets the stored program reference and splits the parameter string.
        /// </summary>
        /// <param name="Program">The stored program associated with this command.</param>
        /// <param name="Params">Comma or space-separated parameters: width height [filled].</param>
        public override void Set(StoredProgram Program, string Params)
        {
            program = Program;
            Parameters = Params.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Validates the parameter count for the rectangle command.
        /// </summary>
        /// <param name="parameterList">Array of parameters to validate.</param>
        /// <exception cref="CommandException">
        /// Thrown if the number of parameters is less than 2 or greater than 3.
        /// </exception>
        public override void CheckParameters(string[] parameterList)
        {
            if (parameterList.Length < 2 || parameterList.Length > 3)
            {
                throw new CommandException(
                    "Rectangle command requires 2 or 3 parameters: width height [filled]. Example: 'rect 100 50 true'."
                );
            }
        }

        /// <summary>
        /// Compiles the rectangle command and determines the filled flag if provided.
        /// </summary>
        public override void Compile()
        {
            CheckParameters(Parameters);

            if (Parameters.Length == 3)
            {
                string flag = Parameters[2].Trim().ToLower();
                filled = flag == "true" || flag == "filled" || flag == "1" || flag == "yes";
            }
        }

        /// <summary>
        /// Executes the rectangle command.
        /// Evaluates width, height, and optional filled flag, then draws the rectangle.
        /// </summary>
        /// <exception cref="CommandException">
        /// Thrown if width or height expressions are invalid,
        /// dimensions are non-positive, filled flag is invalid,
        /// or if the canvas fails to render the rectangle.
        /// </exception>
        public override void Execute()
        {
            int width, height;

            string widthParam = Parameters[0].Trim('<', '>', ' ');
            try
            {
                if (!int.TryParse(widthParam, out width))
                {
                    string eval = program.EvaluateExpression(widthParam)?.Trim() ?? "";
                    if (!int.TryParse(eval, out width))
                        throw new CommandException($"Invalid width expression for rectangle: '{widthParam}' (evaluated as '{eval}')");
                }
            }
            catch (Exception ex) when (!(ex is CommandException))
            {
                throw new CommandException($"Invalid width expression for rectangle: '{widthParam}' ({ex.Message})");
            }

            string heightParam = Parameters[1].Trim('<', '>', ' ');
            try
            {
                if (!int.TryParse(heightParam, out height))
                {
                    string eval = program.EvaluateExpression(heightParam)?.Trim() ?? "";
                    if (!int.TryParse(eval, out height))
                        throw new CommandException($"Invalid height expression for rectangle: '{heightParam}' (evaluated as '{eval}')");
                }
            }
            catch (Exception ex) when (!(ex is CommandException))
            {
                throw new CommandException($"Invalid height expression for rectangle: '{heightParam}' ({ex.Message})");
            }

            if (width <= 0 || height <= 0)
                throw new CommandException($"Rectangle dimensions must be positive: width={width}, height={height}");

            bool isFilled = filled;
            if (Parameters.Length == 3)
            {
                string flagParam = Parameters[2].Trim('<', '>', ' ');
                try
                {
                    string eval = program.EvaluateExpression(flagParam)?.Trim().ToLower() ?? flagParam.ToLower();
                    isFilled = eval == "true" || eval == "1" || eval == "yes" || eval == "filled";
                }
                catch (Exception ex)
                {
                    throw new CommandException($"Invalid filled flag for rectangle: '{flagParam}' ({ex.Message})");
                }
            }

            try
            {
                Canvas.Rect(width, height, isFilled);
            }
            catch (Exception ex)
            {
                throw new CommandException($"Error drawing rectangle ({width}x{height}): {ex.Message}");
            }
        }
    }
}
