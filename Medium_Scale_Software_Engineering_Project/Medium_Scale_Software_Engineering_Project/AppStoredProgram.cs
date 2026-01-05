using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYBooseApp
{
    public class AppStoredProgram : StoredProgram
    {
        public AppStoredProgram(ICanvas canvas) : base(canvas)
        {
        }

        public override void Run()  // Added 'override' keyword
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
                    // Don't call SetSyntaxStatus(false) here!
                    // The syntax is valid, this is a runtime error
                    hadRuntimeError = true;
                    error += "Runtime error: " + e.Message + " at line " + PC + Environment.NewLine;
                }

                if (num > 50000 && PC < 20)
                {
                    throw new StoredProgramException("Program limit reached.");
                }
            }

            // Only check if there were runtime errors, not syntax validity
            if (hadRuntimeError)
            {
                throw new StoredProgramException(error.Trim());
            }
        }
    }
}