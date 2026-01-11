using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Implements the 'PenSize' command to set the width of the pen on the canvas.
    /// Accepts a single parameter, which can be a literal integer or a BOOSE expression.
    /// </summary>
    public class AppPenSize : CommandOneParameter, ICommand
    {
        private ICanvas Canvas;
        private StoredProgram program;
        private string[] Parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppPenSize"/> class with a canvas reference.
        /// </summary>
        /// <param name="canvas">The canvas where the pen width will be applied.</param>
        public AppPenSize(ICanvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// Sets the stored program reference and splits the parameter string into an array.
        /// </summary>
        /// <param name="Program">Reference to the current stored program.</param>
        /// <param name="Params">Parameter string specifying pen width.</param>
        public override void Set(StoredProgram Program, string Params)
        {
            program = Program;
            Parameters = Params.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Validates that exactly one parameter is provided.
        /// </summary>
        /// <param name="parameterList">Parameter array to validate (not used)</param>
        /// <exception cref="CommandException">Thrown if parameter count is not exactly one.</exception>
        public override void CheckParameters(string[] parameterList)
        {
            if (Parameters == null || Parameters.Length != 1)
                throw new CommandException("PenSize command requires exactly 1 parameter.");
        }

        /// <summary>
        /// Compiles the command by checking parameter count.
        /// Evaluation of the pen width is deferred to runtime.
        /// </summary>
        public override void Compile()
        {
            CheckParameters(Parameters);
        }

        /// <summary>
        /// Executes the PenSize command by evaluating the parameter and applying it to the canvas.
        /// </summary>
        /// <exception cref="CommandException">
        /// Thrown if:
        /// <list type="bullet">
        /// <item>The expression cannot be evaluated</item>
        /// <item>The evaluated value is not an integer</item>
        /// <item>The pen size is less than or equal to zero</item>
        /// </list>
        /// </exception>
        public override void Execute()
        {
            int penWidth;

            string widthParam = Parameters[0].Trim('<', '>', ' ');

            // Evaluate the parameter
            penWidth = EvaluatePenWidth(widthParam);

            if (penWidth <= 0)
                throw new CommandException($"Pen size must be a positive integer. Got: {penWidth}");

            // Apply the pen size to the canvas
            if (Canvas is AppCanvas appCanvas)
            {
                appCanvas.penSize(penWidth);
            }
        }

        /// <summary>
        /// Helper method to evaluate the pen width parameter.
        /// </summary>
        /// <param name="param">Parameter string to evaluate</param>
        /// <returns>Integer pen width</returns>
        /// <exception cref="CommandException">Thrown if the parameter cannot be evaluated to a valid integer</exception>
        private int EvaluatePenWidth(string param)
        {
            try
            {
                if (!int.TryParse(param, out int value))
                {
                    string evalResult = program.EvaluateExpression(param)?.Trim() ?? "";
                    if (!int.TryParse(evalResult, out value))
                        throw new CommandException($"Invalid pen size value: '{evalResult}'");
                }
                return value;
            }
            catch (Exception ex)
            {
                throw new CommandException($"Invalid pen size expression: '{param}' ({ex.Message})");
            }
        }
    }
}
