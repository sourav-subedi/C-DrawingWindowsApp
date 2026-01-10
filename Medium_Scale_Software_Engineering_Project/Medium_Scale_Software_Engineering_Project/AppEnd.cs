using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom End command for the MYBooseApp environment.
    /// Extends <see cref="CompoundCommand"/> and handles the termination of
    /// compound commands like If, While, For, and Method.
    /// </summary>
    public class AppEnd : CompoundCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppEnd"/> class
        /// and resets the internal End counter.
        /// </summary>
        public AppEnd()
        {
            ResetEndCounter();
        }

        /// <summary>
        /// Resets the internal static counter of the <see cref="End"/> class to 0.
        /// Uses reflection to find the first static integer field.
        /// </summary>
        private void ResetEndCounter()
        {
            try
            {
                var endType = typeof(End);
                var fields = endType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(int))
                    {
                        field.SetValue(null, 0);
                        break;
                    }
                }
            }
            catch
            {
                // Silently ignore exceptions
            }
        }

        /// <summary>
        /// Compiles the End command by linking it to the corresponding compound command
        /// (If, While, For, or Method) and validates the end syntax.
        /// Sets the <see cref="CompoundCommand.LineNumber"/> and the
        /// <see cref="CompoundCommand.CorrespondingCommand.EndLineNumber"/>.
        /// </summary>
        /// <exception cref="CommandException">
        /// Thrown when the End does not match the expected compound command.
        /// </exception>
        public override void Compile()
        {
            base.CorrespondingCommand = base.Program.Pop();

            if ((base.CorrespondingCommand is If || base.CorrespondingCommand is AppIf) &&
                !base.ParameterList.Contains("if"))
            {
                throw new CommandException("End if expected");
            }

            if ((base.CorrespondingCommand is While || base.CorrespondingCommand is AppWhile) &&
                !base.ParameterList.Contains("while"))
            {
                throw new CommandException("End while expected");
            }

            if ((base.CorrespondingCommand is For || base.CorrespondingCommand is AppFor) &&
                !base.ParameterList.Contains("for"))
            {
                throw new CommandException("End for expected");
            }

            if ((base.CorrespondingCommand is Method || base.CorrespondingCommand is AppMethod) &&
                !base.ParameterList.Contains("method"))
            {
                throw new CommandException("End method expected");
            }

            base.LineNumber = base.Program.Count;
            base.CorrespondingCommand.EndLineNumber = base.LineNumber;
        }

        /// <summary>
        /// Executes the End command by updating the program counter or loop control
        /// based on the type of corresponding command (While, For, AppFor, Method, etc.).
        /// </summary>
        /// <exception cref="CommandException">
        /// Thrown when the loop control variable is not found or the loop direction is invalid.
        /// </exception>
        public override void Execute()
        {
            if (base.CorrespondingCommand is While || base.CorrespondingCommand is AppWhile)
            {
                var whileCmd = base.CorrespondingCommand as ConditionalCommand;
                if (whileCmd != null && whileCmd.Condition)
                {
                    base.Program.PC = base.CorrespondingCommand.LineNumber - 1;
                }
            }
            else if (base.CorrespondingCommand is AppFor appForCmd)
            {
                // Get fresh reference to the loop control variable
                int varIndex = base.Program.FindVariable(appForCmd.LoopControlV.VarName);
                if (varIndex == -1)
                    throw new CommandException("Loop control variable not found");

                Evaluation loopControlV = base.Program.GetVariable(varIndex);
                int currentValue = loopControlV.Value;
                int num = currentValue + appForCmd.Step;

                loopControlV.Value = num;
                base.Program.UpdateVariable(loopControlV.VarName, num);

                if ((appForCmd.From > appForCmd.To && appForCmd.Step >= 0) ||
                    (appForCmd.From < appForCmd.To && appForCmd.Step <= 0))
                {
                    throw new CommandException("Invalid for loop direction");
                }

                if ((appForCmd.Step > 0 && num <= appForCmd.To) ||
                    (appForCmd.Step < 0 && num >= appForCmd.To))
                {
                    base.Program.PC = base.CorrespondingCommand.LineNumber;
                }
            }
            else if (base.CorrespondingCommand is For forCmd)
            {
                int varIndex = base.Program.FindVariable(forCmd.LoopControlV.VarName);
                if (varIndex == -1)
                    throw new CommandException("Loop control variable not found");

                Evaluation loopControlV = base.Program.GetVariable(varIndex);
                int currentValue = loopControlV.Value;
                int num = currentValue + forCmd.Step;

                loopControlV.Value = num;
                base.Program.UpdateVariable(loopControlV.VarName, num);

                if ((forCmd.From > forCmd.To && forCmd.Step >= 0) ||
                    (forCmd.From < forCmd.To && forCmd.Step <= 0))
                {
                    throw new CommandException("Invalid for loop direction");
                }

                if ((forCmd.Step > 0 && num <= forCmd.To) ||
                    (forCmd.Step < 0 && num >= forCmd.To))
                {
                    base.Program.PC = base.CorrespondingCommand.LineNumber;
                }
            }
            else if (base.CorrespondingCommand is AppMethod || base.CorrespondingCommand is Method)
            {
                // Return to caller for methods
                base.Program.PC = base.CorrespondingCommand.ReturnLineNumber;
            }
        }
    }
}
