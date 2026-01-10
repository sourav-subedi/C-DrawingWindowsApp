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
                        field.SetValue(null, 1);
                        break;
                    }
                }
            }
            catch { }
        }

        // OVERRIDE Execute to evaluate the condition!
        public override void Execute()
        {
            base.Execute(); // This calls ConditionalCommand.Execute() which evaluates the expression

            // ConditionalCommand.Execute() already checks Condition and jumps to EndLineNumber if false
            // So we don't need to do anything else here
        }
    }
}