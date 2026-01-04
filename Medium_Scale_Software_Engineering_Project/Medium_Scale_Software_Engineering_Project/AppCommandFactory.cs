using BOOSE;
using MYBooseApp;



namespace MYBooseApp
{

    public class AppCommandFactory : CommandFactory
    {
        public override ICommand MakeCommand(string commandType)
        {
            commandType = commandType.ToLower().Trim();

            if (commandType == "int")
                return new AppInt();

            if (commandType == "moveto")
                return new AppMoveto();

            if (commandType == "circle")
                return new AppCircle();

            if (commandType == "rect" || commandType == "rectangle")
                return new AppRect();

            return base.MakeCommand(commandType);
        }


    }
}
