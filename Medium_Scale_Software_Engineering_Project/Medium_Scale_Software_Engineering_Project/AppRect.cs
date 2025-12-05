using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYBooseApp
{
    /// <summary>
    /// This class overrides the base class to remove the restriction
    /// </summary>
    internal class AppRect : CommandTwoParameters
    {
        /// <summary>
        /// extends the base method to remove restriction
        /// </summary>
        /// <exception cref="CommandException"></exception>

        public override void Execute()
        {
            base.Execute();

            int w = base.Paramsint[0];
            int h = base.Paramsint[1];


            if (w < 0 || h < 0)
                throw new CommandException("Rectangle width/height cannot be negative.");

            base.Canvas.Rect(w, h, false);
        }
    }
}
