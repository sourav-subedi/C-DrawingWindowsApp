using BOOSE;

namespace MYBooseApp
{
    internal class AppParser : Parser
    {
        private readonly StoredProgram _program;

        public AppParser(CommandFactory factory, StoredProgram program)
            : base(factory, program)
        {
            _program = program;   
            program.SetSyntaxStatus(true);
        }

        
        public void Parse(string source)
        {
            _program.Clear();           
            base.ParseProgram(source);  
        }
    }
}