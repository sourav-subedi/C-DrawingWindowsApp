using BOOSE;
using System;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom Write command for the MYBooseApp environment.
    /// Extends <see cref="Command"/> to evaluate expressions or string literals
    /// and output text to the canvas.
    /// </summary>
    public class AppWrite : Command
    {
        /// <summary>
        /// Compiles the Write command. Calls the base <see cref="Command.Compile"/> method.
        /// </summary>
        public override void Compile()
        {
            base.Compile();
        }

        /// <summary>
        /// Executes the Write command by evaluating the expression or string literal
        /// and writing the resulting text to the canvas.
        /// </summary>
        public override void Execute()
        {
            // Get the expression from the parameter list
            string expression = base.ParameterList.Trim();

            // Evaluate the expression
            string result;
            if (base.Program.IsExpression(expression))
            {
                result = base.Program.EvaluateExpressionWithString(expression);
            }
            else
            {
                result = expression.Trim('"'); // Remove quotes if it's a string literal
            }

            // Get the canvas through the stored program
            AppStoredProgram appProgram = base.Program as AppStoredProgram;
            if (appProgram != null)
            {
                ICanvas canvas = appProgram.GetCanvas();
                canvas.WriteText(result);
            }
        }

        /// <summary>
        /// Checks the parameters for the Write command.
        /// No parameter checking is required for this command.
        /// </summary>
        /// <param name="parameterList">The list of parameters passed to the command.</param>
        public override void CheckParameters(string[] parameterList)
        {
            // No parameter checking needed
        }
    }
}
