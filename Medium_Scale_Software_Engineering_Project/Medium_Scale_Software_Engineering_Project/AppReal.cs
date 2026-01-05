using BOOSE;

namespace MYBooseApp
{
    public sealed class AppReal : Evaluation
    {
        private static int instantiationCount;
        private double realValue;

        public new double Value
        {
            get
            {
                return realValue;
            }
            set
            {
                realValue = value;
            }
        }

        public AppReal()
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
            if (!double.TryParse(evaluatedExpression, out realValue))
            {
                throw new StoredProgramException("Invalid real number format.");
            }
            base.Program.UpdateVariable(varName, realValue);
        }
    }
}