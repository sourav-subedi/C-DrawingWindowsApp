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
            int lastPC = -1;
            int sameLineCount = 0;

            while (Commandsleft())
            {
                ICommand command = (ICommand)NextCommand();

                if (command == null)
                {
                    continue;
                }

                try
                {
                    num++;

                    // Track if we're stuck on the same line
                    if (PC == lastPC)
                    {
                        sameLineCount++;
                        if (sameLineCount > 10)
                        {
                            System.Diagnostics.Debug.WriteLine($"STUCK: Iteration {num}, PC = {PC}, Command = {command.GetType().Name}");

                            // Check variable values if it's a while or if
                            if (command is AppWhile || command is While)
                            {
                                var condCmd = command as ConditionalCommand;
                                System.Diagnostics.Debug.WriteLine($"  Condition = {condCmd?.Condition}, BoolValue = {condCmd?.BoolValue}");
                            }
                        }
                    }
                    else
                    {
                        lastPC = PC;
                        sameLineCount = 0;
                    }

                    // Debug output every 1000 iterations
                    //if (num % 1000 == 0)
                    //{
                    //    System.Diagnostics.Debug.WriteLine($"Iteration {num}, PC = {PC}, Command = {command.GetType().Name}");
                    //}

                    command.Execute();
                }
                catch (BOOSEException e)
                {
                    hadRuntimeError = true;
                    error += "Runtime error: " + e.Message + " at line " + PC + Environment.NewLine;
                }

                if (num > 10000000 && PC < 5)
                {
                    throw new StoredProgramException($"Program limit reached - possible infinite loop at line {PC}.");
                }
            }

            if (hadRuntimeError)
            {
                throw new StoredProgramException(error.Trim());
            }
        }
    }
}