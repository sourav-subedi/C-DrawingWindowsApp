using BOOSE;
using MYBooseApp;
using System.Reflection;

namespace MYBooseApp
{
    /// <summary>
    /// Custom command factory for the MYBooseApp environment.
    /// Extends <see cref="CommandFactory"/> to create application-specific commands
    /// and reset internal counters for arrays, booleans, and compound commands.
    /// </summary>
    public class AppCommandFactory : CommandFactory
    {
        /// <summary>
        /// Resets the internal static counter for <see cref="BOOSE.Array"/> commands.
        /// Uses reflection to find the first static integer field and set it to 0.
        /// </summary>
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

        /// <summary>
        /// Resets the internal static counter for <see cref="BOOSE.Boolean"/> commands.
        /// Uses reflection to find the first static integer field and set it to 0.
        /// </summary>
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

        /// <summary>
        /// Resets internal counters for compound commands: If, Else, End, While.
        /// </summary>
        private void ResetCompoundCounters()
        {
            ResetCounter(typeof(If));
            ResetCounter(typeof(Else));
            ResetCounter(typeof(End));
            ResetCounter(typeof(While));
        }

        /// <summary>
        /// Resets the first static integer counter of a given command type to 1.
        /// Used to initialize counters for compound commands.
        /// </summary>
        /// <param name="type">The command type to reset.</param>
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

        /// <summary>
        /// Creates a command instance based on the given command type string.
        /// Overrides the base <see cref="CommandFactory.MakeCommand(string)"/> method.
        /// Resets relevant counters for certain command types before returning the instance.
        /// </summary>
        /// <param name="commandType">The type of command to create (case-insensitive).</param>
        /// <returns>An <see cref="ICommand"/> instance corresponding to the command type.</returns>
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
                ResetBooleanCounter();
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
                ResetBooleanCounter();
                return new AppWhile();
            }
            if (commandType == "for")
            {
                ResetBooleanCounter();
                return new AppFor();
            }

            if (commandType == "method")
            {
                ResetCompoundCounters();
                ResetBooleanCounter();
                return new AppMethod();
            }
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
            if (commandType == "call")
                return new Call();

            return base.MakeCommand(commandType);
        }
    }
}
