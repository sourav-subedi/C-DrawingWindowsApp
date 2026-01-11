using BOOSE;
using System.Diagnostics;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a "for" loop in the BOOSE language with initialization, condition checking, and automatic increment.
    /// Syntax: "for varName = start to end [step increment]".
    /// Example usage: "for i = 0 to 10 step 1" or "for i = 0 to 10" (step defaults to 1).
    /// </summary>
    public class AppFor : ConditionalCommand, ICommand
    {
        /// <summary>
        /// Name of the loop variable (e.g., "i").
        /// </summary>
        private string varName;

        /// <summary>
        /// Expression representing the starting value of the loop variable.
        /// </summary>
        private string startValue;

        /// <summary>
        /// Expression representing the end value of the loop variable.
        /// </summary>
        private string endValue;

        /// <summary>
        /// Expression representing the increment/step value (defaults to "1" if omitted).
        /// </summary>
        private string increment;

        /// <summary>
        /// Index of this for loop command in the stored program.
        /// </summary>
        private int forIndex = -1;

        /// <summary>
        /// Index of the corresponding EndFor command in the stored program.
        /// </summary>
        private int endForIndex = -1;

        /// <summary>
        /// Default constructor required for instantiation via <see cref="AppCommandFactory"/>.
        /// </summary>
        public AppFor() { }

        /// <summary>
        /// Gets the name of the loop variable for use by <see cref="AppEndFor"/>.
        /// </summary>
        /// <returns>The loop variable name.</returns>
        public string GetVarName()
        {
            return varName;
        }

        /// <summary>
        /// Gets the evaluated increment value for use by <see cref="AppEndFor"/>.
        /// </summary>
        /// <returns>The integer increment value.</returns>
        public int GetIncrement()
        {
            return EvaluateIntExpression(increment);
        }

        /// <summary>
        /// Initializes the for loop by parsing the loop parameters.
        /// Expected format: "for varName = start to end [step increment]".
        /// </summary>
        /// <param name="Program">Reference to the stored program being executed.</param>
        /// <param name="Params">String containing loop parameters including variable, start, end, and optional step.</param>
        /// <exception cref="CommandException">Thrown if parameters are invalid or missing.</exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);

            string[] parts = Params.Split(new[] { " to ", " step " }, StringSplitOptions.None);

            if (parts.Length < 2)
                throw new CommandException("For loop requires format: for var = start to end [step increment]");

            string[] initParts = parts[0].Split('=');
            if (initParts.Length != 2)
                throw new CommandException("For loop initialization must be: var = value");

            varName = initParts[0].Trim();
            startValue = initParts[1].Trim();
            endValue = parts[1].Trim();
            increment = parts.Length > 2 ? parts[2].Trim() : "1";
        }

        /// <summary>
        /// Parameter validation is not needed for the for loop command.
        /// </summary>
        /// <param name="parameterList">Array of parameters (not used).</param>
        public override void CheckParameters(string[] parameterList) { }

        /// <summary>
        /// Compilation phase: Records the position of this for loop in the program and pushes it onto the stack.
        /// The corresponding <see cref="AppEndFor"/> will pop this command to establish a bidirectional link.
        /// </summary>
        public override void Compile()
        {
            forIndex = program.Count - 1;

            if (program.Count > 0)
            {
                var lastCmd = ((AppStoredProgram)program).GetCommand(program.Count - 1);
                if (lastCmd != this)
                {
                    for (int i = program.Count - 1; i >= 0; i--)
                    {
                        if (((AppStoredProgram)program).GetCommand(i) == this)
                        {
                            forIndex = i;
                            Debug.WriteLine($"For found itself at index {forIndex}");
                            break;
                        }
                    }
                }
            }

            program.Push(this);
        }

        /// <summary>
        /// Sets the position of the corresponding <see cref="AppEndFor"/> during compilation.
        /// </summary>
        /// <param name="index">Index of the EndFor command.</param>
        public void SetEndForIndex(int index)
        {
            endForIndex = index;
        }

        /// <summary>
        /// Returns the position of this for loop in the stored program.
        /// Used by <see cref="AppEndFor"/> for jumping back to the loop start.
        /// </summary>
        /// <returns>Index of this for loop.</returns>
        public int GetForIndex()
        {
            return forIndex;
        }

        /// <summary>
        /// Executes the for loop: initializes the loop variable, evaluates the loop condition,
        /// and either continues to the loop body or jumps past the loop if the condition is false.
        /// </summary>
        /// <exception cref="BOOSEException">Thrown if the program is not an <see cref="AppStoredProgram"/>.</exception>
        /// <exception cref="CommandException">Thrown if the step value is zero.</exception>
        public override void Execute()
        {
            if (program is not AppStoredProgram extProgram)
                throw new BOOSEException("ForCommand requires AppStoredProgram.");

            if (!program.VariableExists(varName))
            {
                int start = EvaluateIntExpression(startValue);
                var loopVar = new AppInt();
                loopVar.Set(program, $"{varName} = {start}");
                loopVar.VarName = varName;
                loopVar.Value = start;
                program.AddVariable(loopVar);

                Debug.WriteLine($"For loop initialized: {varName} = {start}");
            }

            int currentValue = int.Parse(program.GetVarValue(varName));
            int endVal = EvaluateIntExpression(endValue);
            int stepVal = EvaluateIntExpression(increment);

            Debug.WriteLine($"For at PC={program.PC - 1}: {varName}={currentValue}, checking condition with end={endVal}, step={stepVal}");

            bool shouldContinue;
            if (stepVal > 0) shouldContinue = currentValue <= endVal;
            else if (stepVal < 0) shouldContinue = currentValue >= endVal;
            else throw new CommandException("For loop step cannot be zero.");

            if (shouldContinue) return;

            if (endForIndex == -1)
                throw new BOOSEException("EndFor position not set during compilation.");

            Debug.WriteLine($"Condition false → jumping to PC={endForIndex + 1}");

            program.PC = endForIndex + 1;
            program.DeleteVariable(varName);
        }

        /// <summary>
        /// Evaluates an integer expression with variable substitution.
        /// </summary>
        /// <param name="expr">Expression string (e.g., "x + 5").</param>
        /// <returns>Evaluated integer result.</returns>
        /// <exception cref="CommandException">Thrown if evaluation fails.</exception>
        private int EvaluateIntExpression(string expr)
        {
            expr = expr.Trim();

            expr = System.Text.RegularExpressions.Regex.Replace(expr, @"\b[a-zA-Z_][a-zA-Z0-9_]*\b", match =>
            {
                string varName = match.Value;
                if (program.VariableExists(varName))
                    return program.GetVarValue(varName);
                return varName;
            });

            try
            {
                var dt = new System.Data.DataTable();
                object result = dt.Compute(expr, "");
                return System.Convert.ToInt32(result);
            }
            catch (System.Exception ex)
            {
                throw new CommandException($"Failed to evaluate expression '{expr}': {ex.Message}");
            }
        }
    }
}
