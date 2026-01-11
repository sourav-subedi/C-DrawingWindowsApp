using BOOSE;
using System;
using System.Text.RegularExpressions;

namespace MYBooseApp
{
    /// <summary>
    /// Custom Boolean implementation that avoids the default BOOSE Boolean limitations
    /// by extending the Evaluation class.
    /// Supports declaration and optional initialization using true/false literals
    /// or evaluatable expressions.
    /// Expression results are interpreted numerically, where non-zero represents true
    /// and zero represents false.
    /// </summary>
    public class AppBoolean : Evaluation, ICommand
    {
        private bool boolValue;

        /// <summary>
        /// Initializes a new instance of the AppBoolean class.
        /// Required for creation via the command factory.
        /// </summary>
        public AppBoolean()
        {
        }

        /// <summary>
        /// Gets or assigns the underlying boolean value.
        /// </summary>
        public bool BoolValue
        {
            get => boolValue;
            set => boolValue = value;
        }

        /// <summary>
        /// Provides integer compatibility for the base Evaluation class.
        /// True is represented as 1, while false is represented as 0.
        /// </summary>
        public override int Value
        {
            get => boolValue ? 1 : 0;
            set => boolValue = value != 0;
        }

        /// <summary>
        /// Interprets the declaration parameters.
        /// Supports both simple declarations (e.g., "flag")
        /// and initialized declarations (e.g., "flag = true", "flag = expression").
        /// </summary>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);

            string trimmed = Params.Trim();
            if (trimmed.Contains("="))
            {
                int eqIndex = trimmed.IndexOf('=');
                VarName = trimmed.Substring(0, eqIndex).Trim();
                Expression = trimmed.Substring(eqIndex + 1).Trim();
            }
            else
            {
                VarName = trimmed;
                Expression = "";
            }
        }

        /// <summary>
        /// Registers the boolean variable during compilation and evaluates
        /// any provided initializer.
        /// </summary>
        public override void Compile()
        {
            if (Program == null || string.IsNullOrEmpty(VarName))
                throw new BOOSEException("Invalid AppBoolean state");

            if (!Program.VariableExists(VarName))
            {
                Program.AddVariable(this);

                if (!string.IsNullOrEmpty(Expression))
                {
                    string expr = Expression.Trim('<', '>', ' ').Trim().ToLower();

                    if (expr == "true")
                    {
                        boolValue = true;
                    }
                    else if (expr == "false")
                    {
                        boolValue = false;
                    }
                    else
                    {
                        try
                        {
                            string evalResult = Program.EvaluateExpression(expr);
                            boolValue = !string.IsNullOrEmpty(evalResult)
                                        && double.TryParse(evalResult, out double num)
                                        && num != 0;
                        }
                        catch (Exception ex)
                        {
                            throw new CommandException(
                                $"Failed to evaluate initializer '{Expression}': {ex.Message}");
                        }
                    }
                }
                else
                {
                    boolValue = false; // default value when no initializer is provided
                }
            }
        }

        /// <summary>
        /// Executes boolean expression evaluation and converts the result
        /// into a boolean value.
        /// Numeric results are interpreted such that non-zero values are true
        /// and zero represents false.
        /// </summary>
        public override void Execute()
        {
            if (string.IsNullOrEmpty(Expression))
                return;

            try
            {
                string expr = Expression.Trim('<', '>', ' ').Trim();

                // Substitute variable references with numeric equivalents
                expr = Regex.Replace(expr, @"\b[a-zA-Z_][a-zA-Z0-9_]*\b", match =>
                {
                    string name = match.Value;
                    if (Program.VariableExists(name))
                    {
                        Evaluation varObj = Program.GetVariable(name);
                        if (varObj is AppBoolean b)
                            return b.BoolValue ? "1" : "0";
                        else if (varObj is AppInt i)
                            return i.Value != 0 ? "1" : "0";
                    }
                    return match.Value;
                });

                // Map logical operators to arithmetic operations
                expr = expr.Replace("&&", "*");  // logical AND
                expr = expr.Replace("||", "+");  // logical OR
                expr = expr.Replace("!", "1-");  // logical NOT

                var dt = new System.Data.DataTable();
                dt.CaseSensitive = false;
                double result = Convert.ToDouble(dt.Compute(expr, ""));

                boolValue = result != 0.0;
            }
            catch (Exception ex)
            {
                throw new CommandException($"Boolean evaluation error: {ex.Message}");
            }
        }

        /// <summary>
        /// Parameter validation is not required for this command.
        /// Method is implemented to satisfy interface requirements.
        /// </summary>
        public override void CheckParameters(string[] p) { }
    }
}
