using BOOSE;
using static BOOSE.ConditionalCommand;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom For loop command for the MYBooseApp environment.
    /// Extends <see cref="ConditionalCommand"/> to implement For loop behavior with
    /// loop control variables, from/to values, and optional step.
    /// </summary>
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

        /// <summary>
        /// Gets the loop control variable used in this For loop.
        /// </summary>
        public Evaluation LoopControlV => loopControlV;

        /// <summary>
        /// Gets or sets the starting value of the loop.
        /// </summary>
        public int From
        {
            get { return from; }
            set { from = value; }
        }

        /// <summary>
        /// Gets or sets the ending value of the loop.
        /// </summary>
        public int To
        {
            get { return to; }
            set { to = value; }
        }

        /// <summary>
        /// Gets or sets the step value for the loop increment/decrement.
        /// Default is 1.
        /// </summary>
        public int Step
        {
            get { return step; }
            set { step = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppFor"/> class.
        /// Sets the conditional type to <see cref="conditionalTypes.commFor"/>.
        /// </summary>
        public AppFor()
        {
            CondType = conditionalTypes.commFor;
        }

        /// <summary>
        /// Compiles the For loop command by parsing the expression for the loop
        /// control variable, from/to values, and optional step.
        /// Registers the loop variable in the program if it does not exist.
        /// </summary>
        /// <exception cref="StoredProgramException">
        /// Thrown when the For loop expression is invalid or parameters cannot be parsed.
        /// </exception>
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

        /// <summary>
        /// Executes the For loop command.
        /// Evaluates the from, to, and step expressions and initializes the loop
        /// control variable on the first execution. Subsequent executions are controlled
        /// by the <see cref="AppEnd"/> command handling loop continuation.
        /// </summary>
        /// <exception cref="StoredProgramException">
        /// Thrown when the from, to, or step values cannot be parsed as integers.
        /// </exception>
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
                stepStr = base.Program.EvaluateExpression(stepStr).Trim();
            }

            bool flag2 = int.TryParse(stepStr, out step);

            if (!num || !flag || !flag2)
            {
                throw new StoredProgramException($"Invalid for loop parameters: from='{fromStr}', to='{toStr}', step='{stepStr}'");
            }

            // Initialize loop variable on first execution
            if (firstExecution)
            {
                loopControlV.Value = from;
                base.Program.UpdateVariable(loopVarName, from);
                firstExecution = false;
            }
        }
    }
}
