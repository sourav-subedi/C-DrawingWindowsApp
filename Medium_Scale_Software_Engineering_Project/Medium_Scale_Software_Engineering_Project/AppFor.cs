using BOOSE;

namespace MYBooseApp
{
    public class AppFor : ConditionalCommand
    {
        private int from;
        private int to;
        private int step = 1;
        private string fromStr;
        private string toStr;
        private string stepStr;
        private string loopVarName;
        private Evaluation loopControlV = new Evaluation();
        private bool firstExecution = true;  // Track first execution

        public Evaluation LoopControlV => loopControlV;

        public int From
        {
            get { return from; }
            set { from = value; }
        }

        public int To
        {
            get { return to; }
            set { to = value; }
        }

        public int Step
        {
            get { return step; }
            set { step = value; }
        }

        public AppFor()
        {
            CondType = conditionalTypes.commFor;
        }

        public override void Compile()
        {
            base.Compile();
            string[] array = base.Expression.Split('=');
            string[] array2 = array[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            fromStr = array2[0];
            toStr = array2[2];

            if (array2.Length > 4)
            {
                // Handle negative step: might be split as "step" "-" "2"
                if (array2.Length > 5 && array2[4] == "-")
                {
                    stepStr = "-" + array2[5];
                }
                else
                {
                    stepStr = array2[4];
                }
            }
            else
            {
                stepStr = "1";
            }

            loopVarName = array[0].Trim();
            int num = base.Program.FindVariable(loopVarName);

            if (num != -1)
            {
                loopControlV = base.Program.GetVariable(num);
            }
            else
            {
                loopControlV.VarName = loopVarName;
                loopControlV.Program = base.Program;
                base.Program.AddVariable(loopControlV);
            }

            loopControlV.Expression = fromStr;
        }

        public override void Execute()
        {
            // Evaluate expressions
            if (base.Program.IsExpression(fromStr))
            {
                fromStr = base.Program.EvaluateExpression(fromStr).Trim().ToLower();
            }
            bool num = int.TryParse(fromStr, out from);

            if (base.Program.IsExpression(toStr))
            {
                toStr = base.Program.EvaluateExpression(toStr).Trim().ToLower();
            }
            bool flag = int.TryParse(toStr, out to);

            if (base.Program.IsExpression(stepStr))
            {
                stepStr = base.Program.EvaluateExpression(stepStr).Trim().ToLower();
            }

            // Don't use .ToLower() on stepStr before parsing - it might affect the minus sign
            stepStr = stepStr.Trim();
            bool flag2 = int.TryParse(stepStr, out step);

            if (!num || !flag || !flag2)
            {
                throw new StoredProgramException($"Invalid for loop parameters: from='{fromStr}', to='{toStr}', step='{stepStr}'");
            }

            // CRITICAL: Only initialize loop variable on FIRST execution
            // When PC jumps back from End, this flag will be false
            if (firstExecution)
            {
                loopControlV.Value = from;
                base.Program.UpdateVariable(loopVarName, from);
                firstExecution = false;
            }
        }
    }
}