using BOOSE;
using System.Reflection;

namespace MYBooseApp
{
    public class AppEnd : CompoundCommand
    {
        public AppEnd()
        {
            ResetEndCounter();
        }

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
            catch { }
        }

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
                // Get FRESH reference to the loop control variable from Program
                int varIndex = base.Program.FindVariable(appForCmd.LoopControlV.VarName);
                if (varIndex == -1)
                {
                    throw new CommandException("Loop control variable not found");
                }

                Evaluation loopControlV = base.Program.GetVariable(varIndex);

                int currentValue = loopControlV.Value;
                int num = currentValue + appForCmd.Step;

                // Update the value
                loopControlV.Value = num;
                base.Program.UpdateVariable(loopControlV.VarName, num);

                if ((appForCmd.From > appForCmd.To && appForCmd.Step >= 0) ||
                    (appForCmd.From < appForCmd.To && appForCmd.Step <= 0))
                {
                    throw new CommandException("Invalid for loop direction");
                }

                // Jump back if loop should continue
                if ((appForCmd.Step > 0 && num <= appForCmd.To) ||
                    (appForCmd.Step < 0 && num >= appForCmd.To))
                {
                    base.Program.PC = base.CorrespondingCommand.LineNumber;
                }
            }
            else if (base.CorrespondingCommand is For forCmd)
            {
                // Get FRESH reference to the loop control variable from Program
                int varIndex = base.Program.FindVariable(forCmd.LoopControlV.VarName);
                if (varIndex == -1)
                {
                    throw new CommandException("Loop control variable not found");
                }

                Evaluation loopControlV = base.Program.GetVariable(varIndex);

                int currentValue = loopControlV.Value;
                int num = currentValue + forCmd.Step;

                // Update the value
                loopControlV.Value = num;
                base.Program.UpdateVariable(loopControlV.VarName, num);

                if ((forCmd.From > forCmd.To && forCmd.Step >= 0) ||
                    (forCmd.From < forCmd.To && forCmd.Step <= 0))
                {
                    throw new CommandException("Invalid for loop direction");
                }

                // Jump back if loop should continue
                if ((forCmd.Step > 0 && num <= forCmd.To) ||
                    (forCmd.Step < 0 && num >= forCmd.To))
                {
                    base.Program.PC = base.CorrespondingCommand.LineNumber;
                }
            }
            else if (base.CorrespondingCommand is AppMethod || base.CorrespondingCommand is Method)
            {
                // For method ends, just return to the caller
                // The ReturnLineNumber is set by the Call command
                base.Program.PC = base.CorrespondingCommand.ReturnLineNumber;
            }
        }
    }
}