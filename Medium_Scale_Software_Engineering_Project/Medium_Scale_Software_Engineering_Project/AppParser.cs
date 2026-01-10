using BOOSE;
using MYBooseApp;
using System.Drawing;

namespace MYBooseApp
{
    public class AppParser : Parser
    {
        private StoredProgram storedProgram;

        private ICommandFactory factory;

        public AppParser(CommandFactory factory, StoredProgram program)
            : base(factory, program)
        {
            storedProgram = program;
            this.factory = factory;
        }

        public override ICommand ParseCommand(string line)
        {
            if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("*"))
                return null;

            line = Normalise(line);

            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string commandName = tokens[0];
            string parameterText = string.Join(" ", tokens.Skip(1));

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

        public override void ParseProgram(string programText)
        {
            // Remove BOM if present
            if (programText.Length > 0 && programText[0] == '\uFEFF')
            {
                programText = programText.Substring(1);
            }

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
                        storedProgram.Add(command);

                        // Handle both Method and AppMethod
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
                        else if (command is AppMethod appMethod)
                        {
                            _ = appMethod.MethodName;
                            command = ParseCommand(appMethod.Type + " " + appMethod.MethodName);
                            storedProgram.Remove(command);

                            for (int j = 0; j < appMethod.LocalVariables.Length; j++)
                            {
                                command = ParseCommand(appMethod.LocalVariables[j]);
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