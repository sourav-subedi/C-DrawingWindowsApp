using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Custom command factory for the MYBooseApp environment.
    /// Creates App* commands without relying on static counters.
    /// </summary>
    public class AppCommandFactory : CommandFactory
    {
        /// <summary>
        /// Creates a command instance based on the given command type string.
        /// </summary>
        /// <param name="commandType">The type of command to create (case-insensitive).</param>
        /// <returns>An <see cref="ICommand"/> instance corresponding to the command type.</returns>
        public override ICommand MakeCommand(string commandType)
        {
            commandType = commandType.ToLower().Trim();

            // Variable types
            if (commandType == "evaluation")
                return new AppEvaluation();
            if (commandType == "int")
                return new AppInt();
            if (commandType == "real")
                return new AppReal();
            if (commandType == "boolean")
                return new AppBoolean();
            if (commandType == "array")
                return new AppArray();
            if (commandType == "poke")
                return new AppPoke();
            if (commandType == "peek")
                return new AppPeek();

            // Compound commands
            if (commandType == "if")
                return new AppIf();
            if (commandType == "else")
                return new AppElse();
            if (commandType == "while")
                return new AppWhile();
            if (commandType == "for")
                return new AppFor();
            if (commandType == "method")
                return new AppMethod();
            if (commandType == "end")
                return new AppEnd();

            // Drawing / output commands
            if (commandType == "write")
                return new AppWrite();
            if (commandType == "moveto")
                return new AppMoveto();
            if (commandType == "circle")
                return new AppCircle();
            if (commandType == "rect" || commandType == "rectangle")
                return new AppRect();
            if (commandType == "call")
                return new Call();

            // Fallback to base factory for unknown commands
            return base.MakeCommand(commandType);
        }
    }
}
