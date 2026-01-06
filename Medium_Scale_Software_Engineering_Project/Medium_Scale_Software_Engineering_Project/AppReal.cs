using BOOSE;
using System;

namespace MYBooseApp
{
    public sealed class AppReal : Evaluation
    {
        private static int instantiationCount;
        private double realValue;

        // Shadow the base Value property like Real does
        public new double Value
        {
            get
            {
                Console.WriteLine($"[DEBUG] Getting Value: {realValue}");
                return realValue;
            }
            set
            {
                Console.WriteLine($"[DEBUG] Setting Value to: {value}");
                realValue = value;
            }
        }

        public AppReal()
        {
            instantiationCount++;
            Console.WriteLine($"[DEBUG] AppReal instantiated. Count = {instantiationCount}");
            // Restriction removed: no limit on instantiations
        }

        public override void Compile()
        {
            Console.WriteLine($"[DEBUG] Compiling AppReal variable '{varName}'");
            base.Compile();
            base.Program.AddVariable(this);
            Console.WriteLine($"[DEBUG] Variable '{varName}' added to program");
        }

        public override void Execute()
        {
            Console.WriteLine($"[DEBUG] Executing AppReal variable '{varName}'");
            base.Execute();

            Console.WriteLine($"[DEBUG] Evaluated expression: '{evaluatedExpression}'");

            if (!double.TryParse(evaluatedExpression, out realValue))
            {
                Console.WriteLine($"[ERROR] Failed to parse real value from '{evaluatedExpression}'");
                throw new StoredProgramException("Invalid real number format.");
            }

            Console.WriteLine($"[DEBUG] Parsed real value: {realValue}");

            // CRITICAL: Update with double value
            base.Program.UpdateVariable(varName, realValue);
            Console.WriteLine($"[DEBUG] Program variable '{varName}' updated with value {realValue}");
        }
    }
}
