using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    public class AppMethod : Method
    {
        static AppMethod()
        {
            // Static constructor runs BEFORE any instance is created
            ResetMethodCounterStatic();
        }

        public AppMethod()
        {
            // Also reset in instance constructor to handle multiple methods
            ResetMethodCounterStatic();
        }

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
            catch { }
        }
    }
}