using BOOSE;
using MYBooseApp;
using System.Reflection;

namespace MYBooseApp
{
    public class AppCommandFactory : CommandFactory
    {
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
                        field.SetValue(null, 0);
                        break;
                    }
                }
            }
            catch { }
        }

        public override ICommand MakeCommand(string commandType)
        {
            commandType = commandType.ToLower().Trim();

            if (commandType == "int")
                return new AppInt();

            if (commandType == "real")
                return new AppReal();

            if (commandType == "array")
            {
                ResetArrayCounter();  // Reset before creating
                return new AppArray();
            }

            if (commandType == "poke")
            {
                ResetArrayCounter();  // Reset before creating
                return new AppPoke();
            }

            if (commandType == "peek")
            {
                ResetArrayCounter();  // Reset before creating
                return new AppPeek();
            }

            if (commandType == "write")
                return new AppWrite();

            if (commandType == "moveto")
                return new AppMoveto();

            if (commandType == "circle")
                return new AppCircle();

            if (commandType == "rect" || commandType == "rectangle")
                return new AppRect();

            return base.MakeCommand(commandType);
        }
    }
}