using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYBooseApp { 

    /// <summary>
    /// override base moveto class to remove restriction
    /// </summary>
    public class AppMoveto: CommandTwoParameters
    {
        public override void Execute()
        {
            base.Execute();

            Canvas.MoveTo(Paramsint[0], Paramsint[1]);
        }
    }
}
