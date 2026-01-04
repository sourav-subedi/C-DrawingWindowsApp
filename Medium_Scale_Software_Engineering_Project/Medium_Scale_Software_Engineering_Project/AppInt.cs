using BOOSE;

namespace MYBooseApp
{
    public sealed class AppInt : Evaluation
    {
        private static int instantiationCount;

        public AppInt()
        {
            // Restriction removed: no limit on instantiations
        }

        public override void Compile()
        {
            base.Compile();
            base.Program.AddVariable(this);
        }

        public override void Execute()
        {
            base.Execute();

            if (!int.TryParse(evaluatedExpression, out value))
            {
                if (double.TryParse(evaluatedExpression, out var _))
                {
                    throw new StoredProgramException(/* original message call */ "Fractional values are not allowed for integer variables.");
                }

                throw new StoredProgramException(/* original message call */ "Invalid integer format.");
            }

            base.Program.UpdateVariable(varName, value);
        }
    }
}