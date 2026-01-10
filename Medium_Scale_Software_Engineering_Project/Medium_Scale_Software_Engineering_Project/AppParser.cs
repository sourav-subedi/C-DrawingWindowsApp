using BOOSE;
using System;
using System.Linq;

namespace MYBooseApp
{
    /// <summary>
    /// Custom parser for the MYBooseApp environment.
    /// Extends <see cref="Parser"/> to handle App* commands and custom variable types.
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
        /// Parses a single line of code into a command.
        /// Supports AppInt, AppReal, AppBoolean, and other App* commands.
        /// </summary>
        public override ICommand ParseCommand(string line)
        {
            if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("*"))
                return null;

            line = Normalise(line);
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string commandName = tokens[0];
            string parameterText = string.Join(" ", tokens.Skip(1));

            // Handle assignment statements
            if (tokens.Length > 1 && tokens[1] == "=" &&
                commandName != "appint" && commandName != "int" &&
                commandName != "appreal" && commandName != "real" &&
                commandName != "appboolean" && commandName != "boolean")
            {
                if (!storedProgram.VariableExists(commandName))
                    throw new ParserException("Variable not declared: " + commandName);

                parameterText = commandName + " " + parameterText;
                Evaluation variable = storedProgram.GetVariable(commandName);

                // Infer type from existing variable
                if (variable is AppInt || variable is Int)
                    commandName = "int";
                else if (variable is AppReal || variable is Real)
                    commandName = "real";
                else if (variable is AppBoolean || variable is BOOSE.Boolean)
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
        /// Parses a multi-line program text into the StoredProgram.
        /// Handles comments, blank lines, compound commands, and methods.
        /// </summary>
        public override void ParseProgram(string programText)
        {
            if (programText.Length > 0 && programText[0] == '\uFEFF')
                programText = programText.Substring(1);

            string[] lines = programText.Split('\n');
            string errorText = "";

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(lines[i]) || lines[i].StartsWith("*"))
                    continue;

                try
                {
                    ICommand command = ParseCommand(lines[i]);
                    if (command != null && !(command is CompoundCommand))
                        storedProgram.Add(command);

                    if (command is Method method)
                    {
                        // Optional: handle method local variables if needed
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
                throw new ParserException(errorText.Trim());
        }

        /// <summary>
        /// Adds spaces around operators and removes extra spaces.
        /// </summary>
        private string Normalise(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return "";

            line = line.Trim();
            string[] operators = { "=", "+", "-", "*", "/", "%", ",", "(", ")" };
            foreach (var op in operators)
                line = line.Replace(op, $" {op} ");

            line = line.Replace("\t", " ");
            while (line.Contains("  "))
                line = line.Replace("  ", " ");

            return line.Trim();
        }
    }
}
