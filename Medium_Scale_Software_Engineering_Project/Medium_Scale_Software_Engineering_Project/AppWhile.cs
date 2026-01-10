using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom While loop command for the MYBooseApp environment.
    /// Extends <see cref="CompoundCommand"/> and ensures the internal static
    /// while counter is reset before execution.
    /// </summary>
    public class AppWhile : CompoundCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppWhile"/> class.
        /// Resets the internal while counter and sets the condition type.
        /// </summary>
        public AppWhile()
        {
            ResetWhileCounter();
            CondType = conditionalTypes.commWhile;
        }

        /// <summary>
        /// Resets the internal static counter of the <see cref="While"/> class to 0.
        /// Uses reflection to find the first static integer field.
        /// </summary>
        private void ResetWhileCounter()
        {
            try
            {
                var whileType = typeof(While);
                var fields = whileType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(int))
                    {
                        field.SetValue(null, 0); // Reset counter to 0
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
        /// Executes the While loop command by evaluating its condition.
        /// If the condition is false, jumps to the corresponding End command.
        /// Inherits most behavior from <see cref="ConditionalCommand.Execute"/>.
        /// </summary>
        public override void Execute()
        {
            base.Execute(); // ConditionalCommand.Execute() evaluates condition and handles jump
        }
    }
}
