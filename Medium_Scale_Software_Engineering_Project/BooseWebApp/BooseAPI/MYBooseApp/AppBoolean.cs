using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom Boolean command for the BOOSE scripting environment.
    /// Extends the base <see cref="BOOSE.Boolean"/> class to provide custom
    /// expression evaluation and internal counter reset for boolean variables.
    /// </summary>
    public class AppBoolean : BOOSE.Boolean
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppBoolean"/> class.
        /// Resets the internal Boolean counter to ensure fresh state.
        /// </summary>
        public AppBoolean()
        {
            ResetBooleanCounter();
        }

        /// <summary>
        /// Resets the internal static boolean counter in <see cref="BOOSE.Boolean"/>.
        /// Uses reflection to find and reset the first static integer field.
        /// </summary>
        private void ResetBooleanCounter()
        {
            try
            {
                var booleanType = typeof(BOOSE.Boolean);
                var fields = booleanType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(int))
                    {
                        field.SetValue(null, 0);
                        break;
                    }
                }
            }
            catch
            {
                // Silently ignore exceptions
            }
        }

        /// <summary>
        /// Compiles the Boolean expression using the base implementation.
        /// </summary>
        public override void Compile()
        {
            base.Compile();
        }

        /// <summary>
        /// Executes the Boolean command by evaluating its expression.
        /// Converts logical operators (&&, ||, !) to BOOSE-compatible syntax (AND, OR, NOT).
        /// Sets <see cref="BoolValue"/> and <see cref="value"/> based on the result.
        /// </summary>
        /// <exception cref="CommandException">
        /// Thrown when the evaluated expression is not a valid boolean.
        /// </exception>
        public override void Execute()
        {
            // Call Evaluation.Execute() to get the evaluated expression
            string text = Expression;
            if (base.Program.IsExpression(text))
            {
                // Convert && to AND and || to OR before evaluation
                string convertedExpr = text.Replace("&&", "AND").Replace("||", "OR").Replace("!", "NOT ");
                text = base.Program.EvaluateExpression(convertedExpr).Trim().ToLower();
            }

            // Now check if it's true or false
            if (text.Equals("true") || text.Equals("1") || text.Equals("-1"))
            {
                value = 1;
                BoolValue = true;
                return;
            }
            if (text.Equals("false") || text.Equals("0"))
            {
                value = 0;
                BoolValue = false;
                return;
            }

            throw new CommandException("Invalid boolean value. Expected 'true' or 'false'.");
        }
    }
}
