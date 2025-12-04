using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYBooseApp
{
    public class AppMoveto: CommandTwoParameters
    {
        public override void Execute()
        {
            base.Execute();

            Canvas.MoveTo(Paramsint[0], Paramsint[1]);
        }
    }
}
