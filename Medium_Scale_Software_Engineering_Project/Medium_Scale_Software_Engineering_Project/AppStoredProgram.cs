using BOOSE;
using System;

namespace MYBooseApp
{
    public class AppStoredProgram : StoredProgram
    {
        private ICanvas canvas;
        public AppStoredProgram(ICanvas canvas) : base(canvas)
        {
            this.canvas = canvas;
        }

        public ICanvas GetCanvas()
        {
            return canvas;
        }

        // Override to handle AppReal
        public override void UpdateVariable(string varName, double value)
        {
            Evaluation variable = (Evaluation)GetVariable(varName);

            if (variable is Real)
            {
                ((Real)variable).Value = value;
            }
            else if (variable is AppReal)  // Handle AppReal!
            {
                ((AppReal)variable).Value = value;
            }
            else
            {
                throw new CommandException("Type mismatch, expected a real value");
            }
        }

        // Override to handle AppInt
        public override void UpdateVariable(string varName, int value)
        {
            int index = FindVariable(varName);
            Evaluation evaluation = (Evaluation)GetVariable(index);

            if (evaluation is Int)
            {
                ((Int)evaluation).Value = value;
            }
            else if (evaluation is AppInt)  // Handle AppInt!
            {
                ((AppInt)evaluation).Value = value;
            }
            else
            {
                evaluation.Value = value;
            }
        }

        // Override GetVarValue to handle AppReal
        public override string GetVarValue(string varName)
        {
            int num = FindVariable(varName);
            if (num == -1)
            {
                throw new StoredProgramException("Variable not found");
            }

            Evaluation evaluation = GetVariable(num);

            if (evaluation is Real)
            {
                return ((Real)evaluation).Value.ToString();
            }
            else if (evaluation is AppReal)  // Handle AppReal!
            {
                return ((AppReal)evaluation).Value.ToString();
            }
            else if (evaluation is BOOSE.Boolean)
            {
                if (((BOOSE.Boolean)evaluation).BoolValue)
                    return "true";
                return "false";
            }

            return evaluation.Value.ToString();
        }

        public override void Run()
        {
            int num = 0;
            string error = "";
            bool hadRuntimeError = false;

            while (Commandsleft())
            {
                ICommand command = (ICommand)NextCommand();
                try
                {
                    num++;
                    command.Execute();
                }
                catch (BOOSEException e)
                {
                    hadRuntimeError = true;
                    error += "Runtime error: " + e.Message + " at line " + PC + Environment.NewLine;
                }

                if (num > 50000 && PC < 20)
                {
                    throw new StoredProgramException("Program limit reached.");
                }
            }

            if (hadRuntimeError)
            {
                throw new StoredProgramException(error.Trim());
            }
        }
    }
}