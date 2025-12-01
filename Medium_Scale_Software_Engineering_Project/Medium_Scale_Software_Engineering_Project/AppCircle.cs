using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYBooseApp
{
    /// <summary>
    /// The appCircle class is created with no restrictions in it
    /// </summary>
    internal class AppCircle : CommandOneParameter
    {
        public override void Execute()
        {
            base.Execute();

            int radius = Paramsint[0];

            Canvas.Circle(radius, filled: false);
        }
    }
}
