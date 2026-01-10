using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom Method command for the MYBooseApp environment.
    /// Extends the base <see cref="Method"/> class and ensures the internal
    /// static method counter is reset before any instance is created.
    /// </summary>
    public class AppMethod : Method
    {
        /// <summary>
        /// Static constructor. Resets the internal static counter of the <see cref="Method"/> class
        /// before any instance is created.
        /// </summary>
        static AppMethod()
        {
            ResetMethodCounterStatic();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppMethod"/> class.
        /// Resets the internal static method counter again to handle multiple method instances.
        /// </summary>
        public AppMethod()
        {
            ResetMethodCounterStatic();
        }

        /// <summary>
        /// Resets the internal static counter of the <see cref="Method"/> class to 1.
        /// Uses reflection to find the first static integer field.
        /// </summary>
        private static void ResetMethodCounterStatic()
        {
            try
            {
                var methodType = typeof(Method);
                var fields = methodType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
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
