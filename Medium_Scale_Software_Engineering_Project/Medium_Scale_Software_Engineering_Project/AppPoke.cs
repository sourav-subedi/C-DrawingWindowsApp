using BOOSE;
using System;

namespace MYBooseApp
{
    /// <summary>
    /// Implements the 'poke' command to assign a value to a 1D array element.
    /// Syntax: poke arrayName index = value
    /// Supports integer and real arrays, with index and value evaluated as expressions if needed.
    /// </summary>
    public class AppPoke : Command, ICommand
    {
        private string arrayName;
        private string indexExpr;
        private string valueExpr;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AppPoke() { }

        /// <summary>
        /// Parses the 'poke' command parameters.
        /// Splits the left-hand side into array name and index, right-hand side into value.
        /// </summary>
        /// <param name="Program">Reference to the stored program</param>
        /// <param name="Params">Parameter string in the form: "arrayName index = value"</param>
        /// <exception cref="CommandException">Thrown if syntax is invalid</exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);
            this.Program = Program;

            if (!Params.Contains("="))
                throw new CommandException("Poke requires assignment: poke array index = value");

            int eq = Params.IndexOf('=');
            valueExpr = Params.Substring(eq + 1).Trim();
            string leftSide = Params.Substring(0, eq).Trim();

            string[] parts = leftSide.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                throw new CommandException("Poke requires: arrayName index = value");

            arrayName = parts[0];
            indexExpr = parts[1];
        }

        public override void CheckParameters(string[] parameter) { }

        /// <summary>
        /// Validates array existence and type during compilation.
        /// </summary>
        /// <exception cref="CommandException">Thrown if array does not exist or is not an AppArray</exception>
        public override void Compile()
        {
            if (!Program.VariableExists(arrayName))
                throw new CommandException($"Array '{arrayName}' not declared");

            if (!(Program.GetVariable(arrayName) is AppArray))
                throw new CommandException($"'{arrayName}' is not an array");
        }

        /// <summary>
        /// Executes the 'poke' command at runtime.
        /// Evaluates index and value expressions and assigns the value to the array.
        /// </summary>
        /// <exception cref="CommandException">Thrown for invalid index, value, or array type</exception>
        public override void Execute()
        {
            AppArray array = Program.GetVariable(arrayName) as AppArray;

            int index = EvaluateIndex(indexExpr);
            double value = EvaluateValue(valueExpr);

            // Set the value in the array
            if (array.IsIntArray())
                array.SetIntArray((int)Math.Round(value), index);
            else if (array.IsRealArray())
                array.SetRealArray(value, index);
            else
                throw new CommandException("Unknown array type");
        }

        /// <summary>
        /// Helper method to evaluate the index expression to an integer.
        /// </summary>
        private int EvaluateIndex(string expr)
        {
            if (int.TryParse(expr, out int index))
                return index;

            string evalResult = Program.EvaluateExpression(expr);
            if (!int.TryParse(evalResult, out index))
                throw new CommandException($"Index '{expr}' evaluated to invalid integer");

            return index;
        }

        /// <summary>
        /// Helper method to evaluate the value expression to a double.
        /// </summary>
        private double EvaluateValue(string expr)
        {
            if (double.TryParse(expr, out double value))
                return value;

            string evalResult = Program.EvaluateExpression(expr);
            if (!double.TryParse(evalResult, out value))
                throw new CommandException($"Value '{expr}' evaluated to invalid number");

            return value;
        }
    }
}
