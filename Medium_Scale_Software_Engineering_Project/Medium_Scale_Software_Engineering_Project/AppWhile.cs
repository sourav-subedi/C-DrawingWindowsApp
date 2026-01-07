using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    public class AppWhile : CompoundCommand
    {
        public AppWhile()
        {
            ResetWhileCounter();
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
    }
}