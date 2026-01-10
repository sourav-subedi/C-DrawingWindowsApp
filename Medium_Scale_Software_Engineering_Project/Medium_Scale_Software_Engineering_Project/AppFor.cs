using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom For loop command for the MYBooseApp environment.
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
        private bool firstExecution = true;

        /// <summary>
        /// Gets the loop control variable used in this For loop.
        /// </summary>
        public Evaluation LoopControlV => loopControlV;

        /// <summary>Starting value of the loop</summary>
        public int From { get => from; set => from = value; }

        /// <summary>Ending value of the loop</summary>
        public int To { get => to; set => to = value; }

        /// <summary>Step increment/decrement of the loop (default 1)</summary>
        public int Step { get => step; set => step = value; }

        /// <summary>
        /// Default constructor sets the conditional type to commFor
        /// </summary>
        public AppFor()
        {
            CondType = conditionalTypes.commFor;
        }

        /// <summary>
        /// Sets the loop parameters from parser.
        /// Syntax example: "i = 1 to 10 step 2"
        /// </summary>
        public new void Set(StoredProgram program, string parameters)
        {
            base.Set(program, parameters);

            if (string.IsNullOrWhiteSpace(parameters))
                throw new BOOSEException("For loop requires parameters.");

            string[] parts = parameters.Split('=', 2);
            if (parts.Length != 2)
                throw new BOOSEException("Invalid For loop syntax, expected 'variable = from to to [step x]'");

            loopVarName = parts[0].Trim();
            string[] values = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (values.Length < 3)
                throw new BOOSEException("For loop requires 'from', 'to' and optional 'step'.");

            fromStr = values[0];
            if (values[1].ToLower() != "to")
                throw new BOOSEException("For loop missing 'to' keyword.");
            toStr = values[2];

            stepStr = values.Length >= 5 && values[3].ToLower() == "step" ? values[4] : "1";

            // Register loop variable in program if it does not exist
            int idx = base.Program.FindVariable(loopVarName);
            if (idx != -1)
            {
                loopControlV = base.Program.GetVariable(idx);
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
        /// Compiles the For command.
        /// Pushes itself to stack for AppEnd linking.
        /// </summary>
        public override void Compile()
        {
            base.Program.Push(this);
        }

        /// <summary>
        /// Executes the For loop: evaluates expressions and initializes loop variable.
        /// </summary>
        public override void Execute()
        {
            // Evaluate expressions dynamically
            if (base.Program.IsExpression(fromStr)) fromStr = base.Program.EvaluateExpression(fromStr);
            if (!int.TryParse(fromStr, out from)) throw new BOOSEException($"Invalid 'from' value: {fromStr}");

            if (base.Program.IsExpression(toStr)) toStr = base.Program.EvaluateExpression(toStr);
            if (!int.TryParse(toStr, out to)) throw new BOOSEException($"Invalid 'to' value: {toStr}");

            if (base.Program.IsExpression(stepStr)) stepStr = base.Program.EvaluateExpression(stepStr);
            if (!int.TryParse(stepStr, out step)) throw new BOOSEException($"Invalid 'step' value: {stepStr}");

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
