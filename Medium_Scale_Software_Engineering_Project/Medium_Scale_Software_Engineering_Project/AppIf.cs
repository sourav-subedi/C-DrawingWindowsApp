using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    public class AppIf : CompoundCommand
    {
        public AppIf()
        {
            ResetIfCounter();
        }

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
            catch { }
        }
    }
}