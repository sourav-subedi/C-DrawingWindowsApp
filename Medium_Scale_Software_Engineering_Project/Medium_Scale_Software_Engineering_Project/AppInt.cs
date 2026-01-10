using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom Integer variable command for the MYBooseApp environment.
    /// Extends <see cref="Evaluation"/> to implement integer-specific evaluation,
    /// compilation, and assignment with validation.
    /// </summary>
    public sealed class AppInt : Evaluation
    {
        private static int instantiationCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppInt"/> class.
        /// The restriction on instantiation count has been removed.
        /// </summary>
        public AppInt()
        {
            // Restriction removed: no limit on instantiations
        }

        /// <summary>
        /// Compiles the integer command by invoking the base Compile method
        /// and registering the variable in the program.
        /// </summary>
        public override void Compile()
        {
            base.Compile();
            base.Program.AddVariable(this);
        }

        /// <summary>
        /// Executes the integer command by evaluating the expression and converting
        /// it to an integer value. Updates the program variable with the evaluated value.
        /// </summary>
        /// <exception cref="StoredProgramException">
        /// Thrown when the evaluated expression is not a valid integer or is fractional.
        /// </exception>
        public override void Execute()
        {
            base.Execute();

            if (!int.TryParse(evaluatedExpression, out value))
            {
                if (double.TryParse(evaluatedExpression, out var _))
                {
                    throw new StoredProgramException("Fractional values are not allowed for integer variables.");
                }

                throw new StoredProgramException("Invalid integer format.");
            }

            base.Program.UpdateVariable(varName, value);
        }
    }
}
