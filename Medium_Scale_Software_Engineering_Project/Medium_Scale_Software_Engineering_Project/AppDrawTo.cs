using BOOSE;
using System;

namespace MYBooseApp
{
    /// <summary>
    /// Implements the DrawTo command for the BOOSE language.
    /// Draws a straight line from the current pen position
    /// to the specified X and Y coordinates on the canvas.
    /// </summary>
    /// <remarks>
    /// Coordinate values are evaluated at runtime and may be
    /// numeric literals, variables, or arithmetic expressions.
    /// </remarks>
    public class AppDrawTo : CommandTwoParameters, ICommand
    {
        /// <summary>
        /// Canvas used to perform drawing operations.
        /// </summary>
        private ICanvas Canvas;

        /// <summary>
        /// Reference to the stored program for expression evaluation.
        /// </summary>
        private StoredProgram program;

        /// <summary>
        /// Array containing the raw command parameters.
        /// </summary>
        private string[] Parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDrawTo"/> command.
        /// </summary>
        /// <param name="canvas">
        /// The canvas on which the line will be drawn.
        /// </param>
        public AppDrawTo(ICanvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// Assigns the stored program reference and parses command parameters.
        /// </summary>
        /// <param name="Program">
        /// The current stored program instance.
        /// </param>
        /// <param name="Params">
        /// A parameter string containing X and Y coordinate expressions.
        /// </param>
        public override void Set(StoredProgram Program, string Params)
        {
            program = Program;
            Parameters = Params.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Validates that exactly two parameters are supplied.
        /// </summary>
        /// <param name="parameterList">
        /// The parsed parameter list.
        /// </param>
        /// <exception cref="CommandException">
        /// Thrown when the command does not receive exactly two parameters.
        /// </exception>
        public override void CheckParameters(string[] parameterList)
        {
            if (parameterList.Length != 2)
            {
                throw new CommandException(
                    "DrawTo command requires exactly 2 parameters: x y"
                );
            }
        }

        /// <summary>
        /// Compiles the DrawTo command.
        /// </summary>
        /// <remarks>
        /// Parameter evaluation is deferred until execution time.
        /// </remarks>
        public override void Compile()
        {
            CheckParameters(Parameters);
        }

        /// <summary>
        /// Executes the DrawTo command by evaluating the X and Y expressions
        /// and drawing a line to the resulting coordinates.
        /// </summary>
        /// <exception cref="CommandException">
        /// Thrown when coordinate expressions cannot be evaluated
        /// or do not result in valid integer values.
        /// </exception>
        public override void Execute()
        {
            int x, y;

            // Evaluate X
            string xParam = Parameters[0].Trim('<', '>', ' ');
            try
            {
                if (!int.TryParse(xParam, out x))
                {
                    string xEval = program.EvaluateExpression(xParam)?.Trim() ?? "";
                    if (!int.TryParse(xEval, out x))
                        throw new CommandException($"Invalid X value for DrawTo: '{xEval}'");
                }
            }
            catch (Exception ex)
            {
                throw new CommandException(
                    $"Invalid X expression for DrawTo: '{xParam}' ({ex.Message})"
                );
            }

            // Evaluate Y
            string yParam = Parameters[1].Trim('<', '>', ' ');
            try
            {
                if (!int.TryParse(yParam, out y))
                {
                    string yEval = program.EvaluateExpression(yParam)?.Trim() ?? "";
                    if (!int.TryParse(yEval, out y))
                        throw new CommandException($"Invalid Y value for DrawTo: '{yEval}'");
                }
            }
            catch (Exception ex)
            {
                throw new CommandException(
                    $"Invalid Y expression for DrawTo: '{yParam}' ({ex.Message})"
                );
            }

            // Perform drawing
            try
            {
                Canvas.DrawTo(x, y);
            }
            catch (Exception ex)
            {
                throw new CommandException(
                    $"Error executing DrawTo command: {ex.Message}"
                );
            }
        }
    }
}
