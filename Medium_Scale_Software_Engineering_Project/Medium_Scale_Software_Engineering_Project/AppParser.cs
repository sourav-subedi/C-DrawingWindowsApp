using BOOSE;
using System;

namespace MYBooseApp
{
    /// <summary>
    /// Custom parser extending BOOSE.Parser.
    /// Supports AppInt, variable assignments, and normal commands.
    /// </summary>
    public class AppParser : Parser
    {
        private readonly StoredProgram _program;
        private readonly CommandFactory _factory;

        public AppParser(CommandFactory factory, StoredProgram program)
            : base(factory, program)
        {
            _program = program;
            _factory = factory;
            _program.SetSyntaxStatus(true);
        }

        public void Parse(string source)
        {
            if (string.IsNullOrWhiteSpace(source)) return;

            _program.Clear();
            base.ParseProgram(source);
        }

        public override ICommand ParseCommand(string line)
        {
            line = line.Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith("*"))
                return null;

            int spaceIndex = line.IndexOf(' ');
            string firstWord = (spaceIndex >= 0) ? line.Substring(0, spaceIndex) : line;
            string rest = (spaceIndex >= 0) ? line.Substring(spaceIndex + 1).Trim() : "";

            if (firstWord.ToLower() == "int")
            {
                var appInt = new AppInt(_program); // critical: pass program
                appInt.Set(_program, rest);
                appInt.Compile();
                return appInt;
            }

            if (rest.StartsWith("=") && _program.VariableExists(firstWord))
            {
                var varObj = _program.GetVariable(firstWord) as AppInt;
                if (varObj == null)
                {
                    varObj = new AppInt(_program) { VarName = firstWord };
                    _program.AddVariable(varObj);
                }

                varObj.Expression = rest.Substring(1).Trim();
                varObj.Compile();
                return varObj;
            }



            // --- other commands
            var cmd = _factory.MakeCommand(firstWord);
            cmd.Set(_program, rest);
            cmd.Compile();
            return cmd;
        }
    }
}
