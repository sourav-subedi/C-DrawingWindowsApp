using BOOSE;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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

        // Override to handle AppBoolean
        public override void UpdateVariable(string varName, bool value)
        {
            Evaluation variable = GetVariable(varName);

            if (variable is BOOSE.Boolean)
            {
                ((BOOSE.Boolean)variable).BoolValue = value;
            }
            else if (variable is AppBoolean)  // Handle AppBoolean!
            {
                ((AppBoolean)variable).BoolValue = value;
            }
            else
            {
                throw new CommandException("Type mismatch, expected a boolean value");
            }
        }

        // Override GetVarValue to handle AppReal and AppBoolean
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
            else if (evaluation is AppBoolean)  // Handle AppBoolean first!
            {
                if (((AppBoolean)evaluation).BoolValue)
                    return "true";
                return "false";
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

                if (command == null)
                {
                    continue;
                }

                try
                {
                    num++;

                    // Debug first 10 AppEnd executions
                    if (command is AppEnd && num < 100)
                    {
                        System.Diagnostics.Debug.WriteLine($"\n[{num}] PC={PC}, AppEnd executing");
                        if (VariableExists("count"))
                        {
                            System.Diagnostics.Debug.WriteLine($"  count variable = {GetVarValue("count")}");
                        }

                        var endCmd = command as AppEnd;
                        if (endCmd?.CorrespondingCommand is AppFor appFor)
                        {
                            System.Diagnostics.Debug.WriteLine($"  AppFor.LoopControlV.Value = {appFor.LoopControlV.Value}");
                            System.Diagnostics.Debug.WriteLine($"  AppFor.From = {appFor.From}, To = {appFor.To}, Step = {appFor.Step}");
                        }
                    }

                    command.Execute();

                    // Check PC after AppEnd execution
                    if (command is AppEnd && num < 100)
                    {
                        System.Diagnostics.Debug.WriteLine($"  AFTER Execute: PC = {PC}");
                    }

                    if (num > 200)
                    {
                        System.Diagnostics.Debug.WriteLine("\n=== DEBUG STOP ===");
                        if (VariableExists("count"))
                        {
                            System.Diagnostics.Debug.WriteLine($"Final count = {GetVarValue("count")}");
                        }
                        throw new StoredProgramException($"Debug stop");
                    }
                }
                catch (BOOSEException e)
                {
                    hadRuntimeError = true;
                    error += "Runtime error: " + e.Message + " at line " + PC + Environment.NewLine;
                    break;
                }
            }

            if (hadRuntimeError)
            {
                throw new StoredProgramException(error.Trim());
            }
        }
    }
    }
//```

//Now test both programs:

//**For loop: **
//```
//pen 255,0,0
//moveto 100,100
//for count = 1 to 10 step 2
//	circle count * 10
//end for
//```

//**While loop:**
//```
//moveto 100,100
//int width = 9
//int height = 100
//pen 255,128,0
//while height > 50
//	circle height
//	height = height - 15
//end while
//pen 0,255,0
//moveto 25,25
//rect 100,100