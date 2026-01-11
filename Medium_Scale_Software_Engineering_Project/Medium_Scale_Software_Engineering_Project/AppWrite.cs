using BOOSE;
using System;
using System.Text.RegularExpressions;

namespace MYBooseApp
{
    /// <summary>
    /// Command to write text or variable values on the canvas.
    /// Supports string literals, int and real variables, and expressions.
    /// Concatenates multiple parts using the '+' operator.
    /// </summary>
    public class AppWrite : CommandOneParameter, ICommand
    {
        /// <summary>
        /// Reference to the canvas where text will be drawn.
        /// </summary>
        private ICanvas Canvas;

        /// <summary>
        /// Reference to the stored program to evaluate expressions and fetch variable values.
        /// </summary>
        private StoredProgram program;

        /// <summary>
        /// The raw parameter string passed to the write command.
        /// </summary>
        private string Parameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppWrite"/> class.
        /// </summary>
        /// <param name="canvas">The canvas on which text will be written.</param>
        public AppWrite(ICanvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// Sets the stored program reference and parameter string for this command.
        /// </summary>
        /// <param name="Program">The program executing this command.</param>
        /// <param name="Params">The parameter string, which can be a literal, variable, or expression.</param>
        public override void Set(StoredProgram Program, string Params)
        {
            program = Program;
            Parameter = Params?.Trim() ?? "";
        }

        /// <summary>
        /// Validates that the parameter string is not empty.
        /// </summary>
        /// <param name="parameterList">Array of parameters (not used in this command).</param>
        /// <exception cref="CommandException">Thrown if the parameter is null or empty.</exception>
        public override void CheckParameters(string[] parameterList)
        {
            if (string.IsNullOrWhiteSpace(Parameter))
                throw new CommandException("Write command requires a text or variable parameter.");
        }

        /// <summary>
        /// Compiles the command by trimming leading and trailing spaces from the parameter.
        /// </summary>
        public override void Compile()
        {
            Parameter = Parameter.Trim();
        }

        /// <summary>
        /// Executes the write command.
        /// Evaluates expressions and variables, handles string literals, 
        /// and concatenates parts with the '+' operator.
        /// </summary>
        public override void Execute()
        {
            if (string.IsNullOrWhiteSpace(Parameter))
            {
                Canvas.WriteText("");
                return;
            }

            string expr = Parameter.Trim('<', '>', ' ');

            // Split parameter by '+' operators, respecting escape sequences
            string[] parts = Regex.Split(expr, @"(?<!\\)\+");
            string finalOutput = "";

            foreach (string part in parts)
            {
                string trimmed = part.Trim();

                // Handle string literals in quotes
                if (trimmed.StartsWith("\"") && trimmed.EndsWith("\""))
                {
                    finalOutput += trimmed.Substring(1, trimmed.Length - 2);
                }
                else
                {
                    // Replace variable names with their current values
                    string replaced = Regex.Replace(trimmed, @"\b[a-zA-Z_][a-zA-Z0-9_]*\b", match =>
                    {
                        string name = match.Value;
                        if (program.VariableExists(name))
                        {
                            Evaluation varObj = program.GetVariable(name);
                            if (varObj is AppReal r)
                                return r.RealValue.ToString("G15");
                            else if (varObj is AppInt i)
                                return i.Value.ToString();
                        }
                        return name;
                    });

                    // Evaluate expressions or fallback to literal
                    try
                    {
                        string result = program.EvaluateExpression(replaced);
                        result = result.Trim('<', '>', ' ');
                        finalOutput += result;
                    }
                    catch
                    {
                        finalOutput += replaced;
                    }
                }
            }

            Canvas.WriteText(finalOutput);
        }
    }
}
