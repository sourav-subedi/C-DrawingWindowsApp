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
                // Handle AppFor
                Evaluation loopControlV = appForCmd.LoopControlV;

                int currentValue = loopControlV.Value;
                int num = currentValue + appForCmd.Step;

                if (!base.Program.VariableExists(loopControlV.VarName))
                {
                    throw new CommandException("Loop control variable not found");
                }

                // Update BOTH the LoopControlV object AND the program variable
                loopControlV.Value = num;  // THIS IS CRITICAL!
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
                // Handle original For
                Evaluation loopControlV = forCmd.LoopControlV;
                int num = loopControlV.Value + forCmd.Step;

                if (!base.Program.VariableExists(loopControlV.VarName))
                {
                    throw new CommandException("Loop control variable not found");
                }

                // Update BOTH
                loopControlV.Value = num;  // THIS IS CRITICAL!
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
            else if (base.CorrespondingCommand is Method)
            {
                base.Program.PC = base.CorrespondingCommand.ReturnLineNumber;
            }
        }
    }
}