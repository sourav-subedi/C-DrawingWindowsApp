using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYBooseApp
{
    /// <summary>
    /// the extention of the command factory to add custom made command into it
    /// </summary>
    public class AppCommandFactory : CommandFactory
    {
        /// <summary>
        /// the method overrides the method from the base class to add custom commands
        /// </summary>
        /// <param name="commandType">the string command name</param>
        /// <returns></returns>
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
