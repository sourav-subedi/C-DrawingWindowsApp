using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Custom integer variable implementation without BOOSE restrictions.
    /// Supports both declaration with and without assignment.
    /// </summary>
    public class AppInt : Evaluation
    {
        private int value;

        public override void Compile()
        {
            base.Compile();
            Program.AddVariable(this);
        }

        public override void Execute()
        {
            base.Execute();

            
            if (string.IsNullOrWhiteSpace(evaluatedExpression))
            {
                Program.UpdateVariable(varName, 0);
                return;
            }

            
            if (!int.TryParse(evaluatedExpression, out value))
            {
                if (double.TryParse(evaluatedExpression, out _))
                    throw new StoredProgramException("Invalid int assignment: decimal value.");

                throw new StoredProgramException("Invalid int assignment.");
            }

            Program.UpdateVariable(varName, value);
        }
    }
}
