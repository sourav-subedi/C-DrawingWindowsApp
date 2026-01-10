using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom Else command for the MYBooseApp environment.
    /// Extends the base <see cref="Else"/> class and resets the internal
    /// static Else counter upon instantiation.
    /// </summary>
    public class AppElse : Else
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppElse"/> class
        /// and resets the internal Else counter.
        /// </summary>
        public AppElse()
        {
            ResetElseCounter();
        }

        /// <summary>
        /// Resets the internal static counter of the <see cref="Else"/> class to 1.
        /// Uses reflection to find the first static integer field.
        /// </summary>
        private void ResetElseCounter()
        {
            try
            {
                var elseType = typeof(Else);
                var fields = elseType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
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
                //ignore exceptions
            }
        }
    }
}
