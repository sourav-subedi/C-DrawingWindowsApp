using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    public class AppArray : BOOSE.Array
    {
        public AppArray()
        {
            // Reset the static counter using reflection
            ResetArrayCounter();
        }

        private void ResetArrayCounter()
        {
            try
            {
                var arrayType = typeof(BOOSE.Array);
                var fields = arrayType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
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
                // If reflection fails, ignore
            }
        }

        public override void CheckParameters(string[] parameterList)
        {
            
            base.Parameters = base.ParameterList.Trim().Split(' ');
            if (base.Parameters.Length != 3 && base.Parameters.Length != 4)
            {
                throw new CommandException("Invalid array declaration syntax.");
            }
        }
    }
}