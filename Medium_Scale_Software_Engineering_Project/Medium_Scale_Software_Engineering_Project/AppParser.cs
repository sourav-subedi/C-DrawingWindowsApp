using BOOSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYBooseApp
{
    internal class AppParser : Parser
    {
        public AppParser(CommandFactory Factory, StoredProgram Program) : base(Factory, Program)
        {
        }
    }
}
