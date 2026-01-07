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
                        field.SetValue(null, 1);
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
    if (base.CorrespondingCommand is For && !base.ParameterList.Contains("for"))
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
                System.Diagnostics.Debug.WriteLine($"AppEnd: Jumping from PC {base.Program.PC} back to {base.CorrespondingCommand.LineNumber - 1}");
                System.Diagnostics.Debug.WriteLine($"  While LineNumber = {base.CorrespondingCommand.LineNumber}");
                System.Diagnostics.Debug.WriteLine($"  End LineNumber = {base.LineNumber}");

                base.Program.PC = base.CorrespondingCommand.LineNumber - 1;
            }

            if (base.CorrespondingCommand is While || base.CorrespondingCommand is AppWhile)
    {
        base.Program.PC = base.CorrespondingCommand.LineNumber - 1;
    }
    else if (base.CorrespondingCommand is For)
    {
        For obj = (For)base.CorrespondingCommand;
        Evaluation loopControlV = obj.LoopControlV;
        int num = loopControlV.Value + obj.Step;
        
        if (!base.Program.VariableExists(loopControlV.VarName))
        {
            throw new CommandException("Loop control variable not found");
        }
        
        base.Program.UpdateVariable(loopControlV.VarName, num);
        
        if ((obj.From > obj.To && obj.Step >= 0) || (obj.From < obj.To && obj.Step <= 0))
        {
            throw new CommandException("Invalid for loop direction");
        }
        
        if ((num < obj.To && obj.Step > 0) || (num > obj.To && obj.Step < 0))
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