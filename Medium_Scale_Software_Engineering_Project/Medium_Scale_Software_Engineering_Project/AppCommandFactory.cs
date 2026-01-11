using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Application-specific command factory.
    /// Responsible for creating BOOSE commands supported by MYBooseApp.
    /// Extends the base CommandFactory to register custom drawing,
    /// variable, control-flow, and method-related commands.
    /// </summary>
    public class AppCommandFactory : CommandFactory
    {
        private ICanvas Canvas;

        /// <summary>
        /// Initializes a new instance of the AppCommandFactory.
        /// </summary>
        /// <param name="canvas">
        /// Reference to the application canvas used by drawing commands.
        /// </param>
        public AppCommandFactory(ICanvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// Creates and returns an ICommand instance based on the supplied command type.
        /// </summary>
        /// <param name="CommandType">
        /// The command keyword parsed from the BOOSE program (e.g. "circle", "if", "call").
        /// </param>
        /// <returns>
        /// A new ICommand instance corresponding to the specified command.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the command type is not recognised.
        /// </exception>
        public override ICommand MakeCommand(string CommandType)
        {
            CommandType = CommandType.ToLower().Trim();

            // Drawing commands
            if (CommandType == "circle") return new AppCircle(Canvas);
            if (CommandType == "moveto") return new AppMoveTo(Canvas);
            if (CommandType == "drawto") return new AppDrawTo(Canvas);
            if (CommandType == "pen") return new AppPen(Canvas);
            if (CommandType == "rect") return new AppRect(Canvas);
            if (CommandType == "pensize") return new AppPenSize(Canvas);
            if (CommandType == "tri") return new AppTri(Canvas);
            if (CommandType == "write") return new AppWrite(Canvas);
            if (CommandType == "clear") return new AppClear(Canvas);
            if (CommandType == "reset") return new AppReset(Canvas);

            // Variable declaration commands
            if (CommandType == "int") return new AppInt();
            if (CommandType == "real") return new AppReal();
            if (CommandType == "boolean") return new AppBoolean();

            // Array-related commands
            if (CommandType == "array") return new AppArray();
            if (CommandType == "poke") return new AppPoke();
            if (CommandType == "peek") return new AppPeek();

            // Assignment commands
            if (CommandType == "assign" || CommandType == "set")
            {
                return new AppAsign();
            }

            // Conditional commands
            if (CommandType == "if") return new AppIf();
            if (CommandType == "else") return new AppElse();
            if (CommandType == "end") return new AppEndIf();

            // While loop commands
            if (CommandType == "while") return new AppWhile();
            if (CommandType == "endwhile" || CommandType == "end while") return new AppEndWhile();

            // For loop commands
            if (CommandType == "for") return new AppFor();
            if (CommandType == "endfor" || CommandType == "end for") return new AppEndFor();

            // Method-related commands
            if (CommandType == "method") return new AppMethod();
            if (CommandType == "endmethod" || CommandType == "end method") return new AppEndMethod();
            if (CommandType == "call") return new AppCall();

            return base.MakeCommand(CommandType);
        }
    }
}
