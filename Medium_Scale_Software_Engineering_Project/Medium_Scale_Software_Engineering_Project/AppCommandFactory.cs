using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYBooseApp
{
    internal class AppCommandFactory : CommandFactory
    {
        public override ICommand MakeCommand(string commandType)
        {
            commandType = commandType.ToLower().Trim();

            switch (commandType)
            {
                case "moveto":
                    return new AppMoveto();

                case "circle":
                    return new AppCircle();

                case "rect":
                case "rectangle":
                    return new AppRect();

                default:
                    return base.MakeCommand(commandType);
            }
        }

    }
}
