using BOOSE;
using System;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a real (double-precision) variable in the BOOSE language.
    /// Supports declaration with optional initialization and runtime expression evaluation.
    /// </summary>
    public class AppReal : Evaluation, ICommand
    {
        private double realValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppReal"/> class.
        /// </summary>
        public AppReal() { }

        /// <summary>
        /// Gets or sets the actual double value of the variable.
        /// </summary>
        public double RealValue
        {
            get => realValue;
            set => realValue = value;
        }

        /// <summary>
        /// Gets or sets the integer representation of the variable.
        /// Provides compatibility with BOOSE's <see cref="Evaluation"/> base class.
        /// </summary>
        public override int Value
        {
            get => (int)realValue;
            set => realValue = value;  // Accepts integer assignment for BOOSE compatibility
        }

        /// <summary>
        /// Returns the string representation of the variable suitable for expression evaluation.
        /// Uses up to 15 significant digits in general format.
        /// </summary>
        /// <returns>The string representation of the real value.</returns>
        public override string ToString()
        {
            return realValue.ToString("G15");
        }

        /// <summary>
        /// Sets the program reference and parses the variable declaration.
        /// Supports optional initialization via an expression.
        /// </summary>
        /// <param name="Program">The stored program managing this variable.</param>
        /// <param name="Params">Declaration string (e.g., "pi" or "pi = 3.14").</param>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);

            if (Params.Contains("="))
            {
                int eq = Params.IndexOf('=');
                VarName = Params.Substring(0, eq).Trim();
                Expression = Params.Substring(eq + 1).Trim();
            }
            else
            {
                VarName = Params.Trim();
                Expression = null;
            }
        }

        /// <summary>
        /// Registers the variable with the program and evaluates any initializer expression.
        /// Called during compilation.
        /// </summary>
        public override void Compile()
        {
            if (Program == null)
                throw new BOOSEException("Program not set for real variable.");

            if (string.IsNullOrEmpty(VarName))
                throw new BOOSEException("Real variable name missing.");

            if (!Program.VariableExists(VarName))
            {
                Program.AddVariable(this);

                if (!string.IsNullOrWhiteSpace(Expression))
                {
                    string expr = Expression.Trim('<', '>', ' ');

                    double newValue;

                    if (double.TryParse(expr, out double literal))
                    {
                        newValue = literal;
                    }
                    else
                    {
                        try
                        {
                            string result = Program.EvaluateExpression(expr);
                            if (!double.TryParse(result, out newValue))
                            {
                                throw new CommandException($"Invalid real value '{result}' for '{VarName}'");
                            }
                        }
                        catch (StoredProgramException ex)
                        {
                            throw new CommandException(
                                $"Cannot evaluate expression '{Expression}' for real '{VarName}': {ex.Message}"
                            );
                        }
                    }

                    realValue = newValue;
                }
                else
                {
                    realValue = 0.0;
                }
            }
        }

        /// <summary>
        /// Re-evaluates the variable's expression and updates its value at runtime.
        /// Only executes if an expression was defined.
        /// </summary>
        public override void Execute()
        {
            if (string.IsNullOrWhiteSpace(Expression))
                return;

            string expr = Expression.Trim('<', '>', ' ');

            if (double.TryParse(expr, out double literal))
            {
                realValue = literal;
                return;
            }

            try
            {
                string result = Program.EvaluateExpression(expr);
                if (!double.TryParse(result, out realValue))
                    throw new CommandException($"Invalid real value '{result}' for '{VarName}'");
            }
            catch (StoredProgramException ex)
            {
                throw new CommandException(
                    $"Cannot evaluate expression '{Expression}' for real '{VarName}': {ex.Message}"
                );
            }
        }

        /// <summary>
        /// No additional parameter validation is required for <see cref="AppReal"/>.
        /// </summary>
        /// <param name="parameterList">Array of parameters (not used).</param>
        public override void CheckParameters(string[] parameterList) { }
    }
}
