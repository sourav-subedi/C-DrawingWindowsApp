using BOOSE;
using System;

namespace MYBooseApp
{
    public class AppStoredProgram : StoredProgram
    {
        private readonly ICanvas canvas;

        public AppStoredProgram(ICanvas canvas) : base(canvas)
        {
            this.canvas = canvas;
        }

        public ICanvas GetCanvas() => canvas;

        // Add these so AppIf/Else/While can work
        public int CommandCount => Count;

        public ICommand GetCommand(int index)
        {
            if (index < 0 || index >= Count)
                throw new BOOSEException($"Command index out of range: {index} (total commands: {Count})");

            if (this[index] is not ICommand cmd)
                throw new BOOSEException($"Item at index {index} is not a valid ICommand");

            return cmd;
        }

        public override void UpdateVariable(string varName, double value)
        {
            Evaluation variable = GetVariable(varName);
            switch (variable)
            {
                case AppReal ar: ar.Value = value; break;
                case Real r: r.Value = value; break;
                default: throw new CommandException("Type mismatch, expected a real value");
            }
        }

        public override void UpdateVariable(string varName, int value)
        {
            Evaluation variable = GetVariable(varName);
            switch (variable)
            {
                case AppInt ai: ai.Value = value; break;
                case Int i: i.Value = value; break;
                default: variable.Value = value; break;
            }
        }

        public override void UpdateVariable(string varName, bool value)
        {
            Evaluation variable = GetVariable(varName);
            switch (variable)
            {
                case AppBoolean ab: ab.BoolValue = value; break;
                case BOOSE.Boolean b: b.BoolValue = value; break;
                default: throw new CommandException("Type mismatch, expected a boolean value");
            }
        }

        public override string GetVarValue(string varName)
        {
            Evaluation variable = GetVariable(varName);
            return variable switch
            {
                AppReal ar => ar.Value.ToString(),
                Real r => r.Value.ToString(),
                AppBoolean ab => ab.BoolValue.ToString().ToLower(),
                BOOSE.Boolean b => b.BoolValue.ToString().ToLower(),
                _ => variable.Value.ToString()
            };
        }

        public override void Run()
        {
            string error = "";
            bool hadRuntimeError = false;

            while (Commandsleft())
            {
                ICommand command = (ICommand)NextCommand();
                if (command == null) continue;

                try
                {
                    command.Execute();
                }
                catch (BOOSEException e)
                {
                    hadRuntimeError = true;
                    error += $"Runtime error: {e.Message} at line {PC}{Environment.NewLine}";
                    break;
                }
            }

            if (hadRuntimeError)
                throw new StoredProgramException(error.Trim());
        }
    }
}
