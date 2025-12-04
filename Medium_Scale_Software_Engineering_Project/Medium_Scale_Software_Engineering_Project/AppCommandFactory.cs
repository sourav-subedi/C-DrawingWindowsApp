using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYBooseApp
{
    public class AppCommandFactory : CommandFactory
    {
        public override ICommand MakeCommand(string commandType)
        {
            commandType = commandType.ToLower().Trim();

            if (commandType == "moveto")
            {
                return new AppMoveto();
            }
            else if (commandType == "circle")
            {
                return new AppCircle();
            }
            else if (commandType == "rect" || commandType == "rectangle")
            {
                return new AppRect();
            }
            else
            {
                return base.MakeCommand(commandType);
            }
        }

    }
}
