using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom If command for the MYBooseApp environment.
    /// Extends the base <see cref="CompoundCommand"/> class without static counters.
    /// </summary>
    public class AppIf : CompoundCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppIf"/> class.
        /// </summary>
        public AppIf()
        {
            // No static counter, so nothing to reset
        }

        /// <summary>
        /// Compiles the If command by evaluating its condition expression.
        /// </summary>
        public override void Compile()
        {
            base.Compile();
            // Optionally, you can evaluate and store the condition if needed:
            if (!string.IsNullOrEmpty(Expression))
            {
                Condition = Program.EvaluateExpression(Expression).Trim().ToLower() == "true";
            }
        }

        /// <summary>
        /// Executes the If command by checking its condition.
        /// </summary>
        public override void Execute()
        {
            // Evaluate the condition at runtime
            if (!string.IsNullOrEmpty(Expression))
            {
                Condition = Program.EvaluateExpression(Expression).Trim().ToLower() == "true";
            }
        }
    }
}
