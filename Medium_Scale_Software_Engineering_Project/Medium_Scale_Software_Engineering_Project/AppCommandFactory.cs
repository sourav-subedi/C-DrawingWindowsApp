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

            if (commandType == "real")
                return new AppReal();

            if (commandType == "moveto")
                return new AppMoveto();

            if (commandType == "circle")
                return new AppCircle();

            if (commandType == "rect" || commandType == "rectangle")
                return new AppRect();

            if (commandType == "write")
                return new AppWrite();

            if (commandType == "array")
                return new AppArray();

            if (commandType == "poke")
                return new AppPoke();

            if (commandType == "peek")
                return new AppPeek();

            return base.MakeCommand(commandType);
        }


    }
}
