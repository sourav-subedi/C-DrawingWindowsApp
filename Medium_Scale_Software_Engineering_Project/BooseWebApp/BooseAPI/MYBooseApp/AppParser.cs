using BOOSE;
using MYBooseApp;
using System.Drawing;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom parser for the MYBooseApp environment.
    /// Extends the base <see cref="Parser"/> to handle App* commands,
    /// custom variable types, and expression normalization.
    /// </summary>
    public class AppParser : Parser
    {
        private StoredProgram storedProgram;
        private ICommandFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppParser"/> class.
        /// </summary>
        /// <param name="factory">The command factory used to create commands.</param>
        /// <param name="program">The stored program to parse and populate.</param>
        public AppParser(CommandFactory factory, StoredProgram program)
            : base(factory, program)
        {
            storedProgram = program;
            this.factory = factory;
        }

        /// <summary>
        /// Parses a single line of program text into an <see cref="ICommand"/> object.
        /// Handles assignment statements, variable type inference, and App* commands.
        /// </summary>
        /// <param name="line">The line of code to parse.</param>
        /// <returns>An <see cref="ICommand"/> representing the parsed line, or null if the line is empty or a comment.</returns>
        /// <exception cref="ParserException">Thrown if a variable is undeclared or has an unknown type.</exception>
        public override ICommand ParseCommand(string line)
        {
            if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("*"))
                return null;

            line = Normalise(line);

            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string commandName = tokens[0];
            string parameterText = string.Join(" ", tokens.Skip(1));

            // Handle assignment statements
            if (tokens.Length > 1 &&
                tokens[1] == "=" &&
                commandName != "appint" &&
                commandName != "int" &&
                commandName != "real" &&
                commandName != "boolean")
            {
                if (!storedProgram.VariableExists(commandName))
                    throw new ParserException("Variable not declared: " + commandName);

                parameterText = commandName + " " + parameterText;
                Evaluation variable = storedProgram.GetVariable(commandName);

                if (variable is Int || variable is AppInt)
                    commandName = "int";
                else if (variable is Real || variable is AppReal)
                    commandName = "real";
                else if (variable is BOOSE.Boolean)
                    commandName = "boolean";
                else
                    throw new ParserException("Unknown variable type");
            }

            ICommand command = factory.MakeCommand(commandName);
            command.Set(storedProgram, parameterText);
            command.Compile();

            return command;
        }

        /// <summary>
        /// Parses a multi-line program text into the <see cref="StoredProgram"/>.
        /// Handles comments, blank lines, compound commands, and method commands.
        /// </summary>
        /// <param name="programText">The program text to parse.</param>
        /// <exception cref="ParserException">Thrown when syntax errors are detected.</exception>
        public override void ParseProgram(string programText)
        {
            // Remove BOM if present
            if (programText.Length > 0 && programText[0] == '\uFEFF')
                programText = programText.Substring(1);

            programText += "\n";
            string errorText = "";
            string[] lines = programText.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(lines[i]) || lines[i].StartsWith("*"))
                    continue;

                try
                {
                    ICommand command = ParseCommand(lines[i]);

                    if (command != null)
                    {
                        if (!(command is CompoundCommand))
                        {
                            storedProgram.Add(command);
                        }

                        // Handle method commands
                        if (command is Method method)
                        {
                            _ = method.MethodName;
                            command = ParseCommand(method.Type + " " + method.MethodName);
                            storedProgram.Remove(command);

                            for (int j = 0; j < method.LocalVariables.Length; j++)
                            {
                                command = ParseCommand(method.LocalVariables[j]);
                                ((Evaluation)command).Local = true;
                                storedProgram.Remove(command);
                            }
                        }
                    }
                }
                catch (BOOSEException ex)
                {
                    if (!string.IsNullOrEmpty(ex.Message))
                    {
                        errorText += ex.Message + " at line " + (i + 1) + "\n";
                        storedProgram.SetSyntaxStatus(false);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(errorText))
            {
                throw new ParserException(errorText.Trim());
            }
        }

        /// <summary>
        /// Normalizes a line of code by adding spaces around operators and delimiters
        /// and removing extra whitespace. Prepares the line for tokenization.
        /// </summary>
        /// <param name="line">The line of code to normalize.</param>
        /// <returns>A normalized string ready for parsing.</returns>
        private string Normalise(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return string.Empty;

            line = line.Trim();

            // Add spaces around operators and delimiters
            line = line.Replace("=", " = ");
            line = line.Replace("+", " + ");
            line = line.Replace("-", " - ");
            line = line.Replace("*", " * ");
            line = line.Replace("/", " / ");
            line = line.Replace("%", " % ");
            line = line.Replace(",", " , ");
            line = line.Replace("(", " ( ");
            line = line.Replace(")", " ) ");
            line = line.Replace("\t", " ");

            // Clean up multiple spaces
            while (line.Contains("  "))
                line = line.Replace("  ", " ");

            return line.Trim();
        }
    }
}
