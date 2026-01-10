using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    public class AppBoolean : BOOSE.Boolean
    {
        public AppBoolean()
        {
            ResetBooleanCounter();
        }

        private void ResetBooleanCounter()
        {
            try
            {
                var booleanType = typeof(BOOSE.Boolean);
                var fields = booleanType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(int))
                    {
                        field.SetValue(null, 0);
                        break;
                    }
                }
            }
            catch { }
        }

        public override void Compile()
        {
            base.Compile();
        }

        public override void Execute()
        {
            base.Execute();
        }
    }
}