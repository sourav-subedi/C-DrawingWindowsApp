using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    public class AppWhile : CompoundCommand
    {
        public AppWhile()
        {
            ResetWhileCounter();
            CondType = conditionalTypes.commWhile;
        }

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
                        field.SetValue(null, 0); // Changed from 1 to 0
                        break;
                    }
                }
            }
            catch { }
        }

        public override void Execute()
        {
            base.Execute(); // Calls ConditionalCommand.Execute() which evaluates condition
            // ConditionalCommand.Execute() jumps to EndLineNumber if condition is false
        }
    }
}