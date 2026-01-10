using BOOSE;
using System;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom Real (floating-point) variable for the MYBooseApp environment.
    /// Extends <see cref="Evaluation"/> to implement real-number-specific evaluation,
    /// compilation, and assignment with validation.
    /// </summary>
    public sealed class AppReal : Evaluation
    {
        private static int instantiationCount;
        private double realValue;

        /// <summary>
        /// Gets or sets the real value of this variable.
        /// Shadowing the base <see cref="Evaluation.Value"/> property.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AppReal"/> class.
        /// Increments the instantiation counter. No limit on instantiations.
        /// </summary>
        public AppReal()
        {
            instantiationCount++;
            Console.WriteLine($"[DEBUG] AppReal instantiated. Count = {instantiationCount}");
        }

        /// <summary>
        /// Compiles the real variable command by invoking the base Compile method
        /// and registering the variable in the program.
        /// </summary>
        public override void Compile()
        {
            Console.WriteLine($"[DEBUG] Compiling AppReal variable '{varName}'");
            base.Compile();
            base.Program.AddVariable(this);
            Console.WriteLine($"[DEBUG] Variable '{varName}' added to program");
        }

        /// <summary>
        /// Executes the real variable command by evaluating the expression and converting
        /// it to a double value. Updates the program variable with the evaluated value.
        /// </summary>
        /// <exception cref="StoredProgramException">
        /// Thrown when the evaluated expression is not a valid real number.
        /// </exception>
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

            // Update program variable with the parsed double value
            base.Program.UpdateVariable(varName, realValue);
            Console.WriteLine($"[DEBUG] Program variable '{varName}' updated with value {realValue}");
        }
    }
}
