using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Custom Circle command for the BOOSE language.
    /// Draws a circle on the canvas using a radius that can be
    /// a literal value or an evaluated expression at runtime.
    /// Optionally supports a filled flag.
    /// </summary>
    public class AppCircle : CommandTwoParameters, ICommand
    {
        private ICanvas Canvas;
        private StoredProgram program;
        private string[] Parameters;
        private bool filled = false;

        /// <summary>
        /// Creates a new AppCircle command using the provided canvas.
        /// </summary>
        /// <param name="canvas">Canvas used to draw the circle.</param>
        public AppCircle(ICanvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// Assigns the current StoredProgram and parses the parameter string.
        /// Parameters are split by comma to preserve expressions.
        /// </summary>
        /// <param name="Program">The current stored program.</param>
        /// <param name="Params">
        /// Parameter string containing radius and optional filled flag.
        /// </param>
        public override void Set(StoredProgram Program, string Params)
        {
            program = Program;

            string[] parts = Params.Split(',');

            if (parts.Length < 1 || parts.Length > 2)
            {
                throw new CommandException(
                    "Circle command requires 1 or 2 parameters: radius [filled]."
                );
            }

            Parameters = new string[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                Parameters[i] = parts[i].Trim();
            }
        }

        /// <summary>
        /// Checks that the number of parameters is valid.
        /// </summary>
        /// <param name="parameterList">List of parameters passed to the command.</param>
        public override void CheckParameters(string[] parameterList)
        {
            if (parameterList.Length < 1 || parameterList.Length > 2)
            {
                throw new CommandException(
                    "Circle command requires 1 or 2 parameters: radius [filled]."
                );
            }
        }

        /// <summary>
        /// Compiles the command and determines whether the circle
        /// should be filled if a second parameter is provided.
        /// </summary>
        public override void Compile()
        {
            CheckParameters(Parameters);

            if (Parameters.Length == 2)
            {
                string flag = Parameters[1].Trim().ToLower();
                filled = flag == "true" || flag == "filled" || flag == "1" || flag == "yes";
            }
        }

        /// <summary>
        /// Executes the Circle command.
        /// Evaluates the radius expression at runtime and optionally
        /// evaluates a filled flag before drawing the circle.
        /// </summary>
        public override void Execute()
        {
            string radiusParam = Parameters[0].Trim('<', '>', ' ');
            double radiusValue;

            if (!double.TryParse(radiusParam, out radiusValue))
            {
                try
                {
                    string evalResult = program.EvaluateExpression(radiusParam);
                    radiusValue = double.Parse(evalResult);
                }
                catch
                {
                    throw new CommandException(
                        $"Invalid radius expression for circle: '{radiusParam}'"
                    );
                }
            }

            int radius = (int)Math.Round(radiusValue);

            if (radius <= 0)
                throw new CommandException("Circle radius must be greater than zero.");

            bool isFilled = false;

            if (Parameters.Length == 2)
            {
                string flagParam = Parameters[1].Trim('<', '>', ' ');
                if (!bool.TryParse(flagParam, out isFilled))
                {
                    try
                    {
                        string flagEval = program.EvaluateExpression(flagParam).ToLower();
                        isFilled = flagEval == "1" || flagEval == "true"
                                   || flagEval == "yes" || flagEval == "filled";
                    }
                    catch
                    {
                        throw new CommandException(
                            $"Invalid filled flag for circle: '{flagParam}'"
                        );
                    }
                }
            }

            Canvas.Circle(radius, isFilled);
        }
    }
}
