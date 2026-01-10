using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom If command for the MYBooseApp environment.
    /// Extends the base <see cref="CompoundCommand"/> class and resets the internal
    /// If counter upon instantiation.
    /// </summary>
    public class AppIf : CompoundCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppIf"/> class
        /// and resets the internal If counter.
        /// </summary>
        public AppIf()
        {
            ResetIfCounter();
        }

        /// <summary>
        /// Resets the internal static counter of the <see cref="If"/> class to 1.
        /// Uses reflection to find the first static integer field.
        /// </summary>
        private void ResetIfCounter()
        {
            try
            {
                var ifType = typeof(If);
                var fields = ifType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(int))
                    {
                        field.SetValue(null, 1);
                        break;
                    }
                }
            }
            catch
            {
                // Silently ignore exceptions
            }
        }
    }
}
