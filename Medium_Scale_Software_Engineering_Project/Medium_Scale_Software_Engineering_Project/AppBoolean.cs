using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom Boolean command for the BOOSE scripting environment.
    /// Extends the base <see cref="BOOSE.Boolean"/> class to provide custom
    /// expression evaluation without relying on static counters.
    /// </summary>
    public class AppBoolean : BOOSE.Boolean
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppBoolean"/> class.
        /// </summary>
        public AppBoolean()
        {
            // No static counter, fresh instance
            value = 0;
            BoolValue = false;
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
            // Get the raw expression
            string text = Expression;

            // Evaluate if it's an expression
            if (base.Program.IsExpression(text))
            {
                // Convert C# style logical operators to BOOSE equivalents
                string convertedExpr = text.Replace("&&", "AND")
                                           .Replace("||", "OR")
                                           .Replace("!", "NOT ");
                text = base.Program.EvaluateExpression(convertedExpr).Trim().ToLower();
            }

            // Convert result to boolean
            switch (text)
            {
                case "true":
                case "1":
                case "-1":
                    value = 1;
                    BoolValue = true;
                    break;
                case "false":
                case "0":
                    value = 0;
                    BoolValue = false;
                    break;
                default:
                    throw new CommandException($"Invalid boolean value '{text}'. Expected 'true' or 'false'.");
            }
        }
    }
}
