using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    public class AppElse : Else
    {
        public AppElse()
        {
            ResetElseCounter();
        }

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
            catch { }
        }
    }
}