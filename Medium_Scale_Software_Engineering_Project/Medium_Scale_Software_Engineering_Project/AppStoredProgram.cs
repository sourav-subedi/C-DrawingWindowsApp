using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOSEDrawingApp
{
    public class AppStoredProgram : StoredProgram
    {
        public AppStoredProgram(ICanvas canvas) : base(canvas)
        {
        }

        public void Run()
        {
            int num = 0;
            string error = "";
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
                    SetSyntaxStatus(false);
                    error += "Runtime error: " + e.Message + " at line " + PC + Environment.NewLine;
                }
                if (num > 50000 && PC < 20)
                {
                    throw new StoredProgramException("Program limit reached.");
                }

            }

            if (!IsValidProgram())
            {
                throw new StoredProgramException("Not a valid program.");
            }
        }
    }
}