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

        private void ResetBooleanCounter()
        {
            try
            {
                var boolType = typeof(BOOSE.Boolean);
                var fields = boolType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
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

        private void ResetCompoundCounters()
        {
            ResetCounter(typeof(If));
            ResetCounter(typeof(Else));
            ResetCounter(typeof(End));
            ResetCounter(typeof(While));
        }

        private void ResetCounter(System.Type type)
        {
            try
            {
                var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
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

        public override ICommand MakeCommand(string commandType)
        {
            commandType = commandType.ToLower().Trim();

            // Handle evaluation/assignment - this is what handles "height = height - 15"
            if (commandType == "evaluation")
                return new AppEvaluation();

            if (commandType == "int")
                return new AppInt();
            if (commandType == "real")
                return new AppReal();
            if (commandType == "boolean")
            {
                ResetBooleanCounter();
                return new AppBoolean();
            }
            if (commandType == "array")
            {
                ResetArrayCounter();
                return new AppArray();
            }
            if (commandType == "poke")
            {
                ResetArrayCounter();
                return new AppPoke();
            }
            if (commandType == "peek")
            {
                ResetArrayCounter();
                return new AppPeek();
            }
            if (commandType == "if")
            {
                ResetCompoundCounters();
                return new AppIf();
            }
            if (commandType == "else")
            {
                ResetCompoundCounters();
                return new AppElse();
            }
            if (commandType == "while")
            {
                ResetCompoundCounters();
                return new AppWhile();
            }
            if (commandType == "for")
                return new AppFor();
            if (commandType == "end")
            {
                ResetCompoundCounters();
                return new AppEnd();
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