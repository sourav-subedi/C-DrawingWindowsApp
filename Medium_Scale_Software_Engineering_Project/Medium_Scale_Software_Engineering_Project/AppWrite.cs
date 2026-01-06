using BOOSE;
using System;

namespace MYBooseApp
{
    public class AppWrite : Command
    {
        public override void Compile()
        {
            base.Compile();
        }

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

        public override void CheckParameters(string[] parameterList)
        {
            // No parameter checking needed
        }
    }
}