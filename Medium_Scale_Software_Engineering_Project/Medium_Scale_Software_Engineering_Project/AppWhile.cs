using BOOSE;
using System.Diagnostics;

namespace MYBooseApp
{
    /// <summary>
    /// Implements a while loop command for BOOSE.
    /// Repeatedly executes the loop body as long as the condition expression evaluates to true.
    /// Supports nested loops using forward-scanning and an internal counter variable.
    /// Syntax: "while condition"
    /// </summary>
    public class AppWhile : ConditionalCommand, ICommand
    {
        /// <summary>
        /// The condition expression to evaluate for continuing the loop.
        /// </summary>
        private string condition;

        /// <summary>
        /// The index of this AppWhile command in the program.
        /// Used by AppEndWhile to jump back to the loop start.
        /// </summary>
        private int whileIndex = -1;

        /// <summary>
        /// Unique internal counter variable name for this loop instance.
        /// </summary>
        private string internalCounterVar;

        /// <summary>
        /// Default constructor required for CommandFactory instantiation.
        /// </summary>
        public AppWhile() { }

        /// <summary>
        /// Sets the stored program reference and parses the loop condition.
        /// Creates a unique internal counter variable for this instance.
        /// </summary>
        /// <param name="Program">Reference to the stored program.</param>
        /// <param name="Params">Condition expression for the while loop.</param>
        /// <exception cref="CommandException">Thrown if the condition is missing or empty.</exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);
            condition = Params?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(condition))
                throw new CommandException("While command requires a condition expression.");

            internalCounterVar = $"__while_counter_{GetHashCode()}";
        }

        /// <summary>
        /// Parameter validation for ICommand interface. No parameters are required for while.
        /// </summary>
        /// <param name="parameterList">Array of parameters (not used).</param>
        public override void CheckParameters(string[] parameterList) { }

        /// <summary>
        /// Compilation phase: Records this command's index in the program.
        /// Pushes this command onto the stack for linking with AppEndWhile.
        /// </summary>
        public override void Compile()
        {
            whileIndex = program.Count - 1;

            if (program.Count > 0)
            {
                var lastCmd = ((AppStoredProgram)program).GetCommand(whileIndex);
                if (lastCmd != this)
                {
                    for (int i = program.Count - 1; i >= 0; i--)
                    {
                        if (((AppStoredProgram)program).GetCommand(i) == this)
                        {
                            whileIndex = i;
                            break;
                        }
                    }
                }
            }

            program.Push(this);
        }

        /// <summary>
        /// Executes the while loop by evaluating the condition and managing loop iterations.
        /// Handles nested loops with forward-scanning to the matching EndWhile command.
        /// </summary>
        /// <exception cref="BOOSEException">
        /// Thrown if AppStoredProgram is not used or no matching EndWhile is found.
        /// </exception>
        public override void Execute()
        {
            if (program is not AppStoredProgram extProgram)
                throw new BOOSEException("AppWhile requires AppStoredProgram.");

            if (!program.VariableExists(internalCounterVar))
            {
                var counterVar = new AppInt();
                counterVar.Set(program, $"{internalCounterVar} = 0");
                program.AddVariable(counterVar);
            }
            else
            {
                int currentValue = int.Parse(program.GetVarValue(internalCounterVar));
                program.UpdateVariable(internalCounterVar, currentValue + 1);
            }

            bool isTrue = EvaluateCondition(condition);

            if (isTrue)
                return; // Continue to loop body

            program.DeleteVariable(internalCounterVar);

            int myPosition = program.PC - 1;
            int nestingLevel = 0;

            for (int i = myPosition + 1; i < extProgram.CommandCount; i++)
            {
                var cmd = extProgram.GetCommand(i);

                if (cmd is AppWhile)
                    nestingLevel++;
                else if (cmd is AppEndWhile)
                {
                    if (nestingLevel == 0)
                    {
                        program.PC = i + 1;
                        return;
                    }
                    nestingLevel--;
                }
            }

            throw new BOOSEException("No matching 'end while' found for this while loop.");
        }

        /// <summary>
        /// Gets the program index of this while command.
        /// Used by AppEndWhile for loop control.
        /// </summary>
        /// <returns>The index of this AppWhile in the program.</returns>
        public int GetWhileIndex()
        {
            return whileIndex;
        }

        /// <summary>
        /// Evaluates a boolean condition expression with variable substitution.
        /// Supports comparison and logical operators.
        /// </summary>
        /// <param name="cond">The condition expression to evaluate.</param>
        /// <returns>True if the condition evaluates to true, otherwise false.</returns>
        /// <exception cref="CommandException">Thrown if the condition cannot be evaluated.</exception>
        private bool EvaluateCondition(string cond)
        {
            string expr = cond.Trim();

            expr = System.Text.RegularExpressions.Regex.Replace(expr, @"\b[a-zA-Z_][a-zA-Z0-9_]*\b", match =>
            {
                string varName = match.Value;
                if (program.VariableExists(varName))
                    return program.GetVarValue(varName);
                return varName;
            });

            expr = expr.Replace("&&", " AND ")
                       .Replace("||", " OR ")
                       .Replace("!", "NOT ");

            try
            {
                var dt = new System.Data.DataTable();
                object result = dt.Compute(expr, "");
                return System.Convert.ToBoolean(result);
            }
            catch (System.Exception ex)
            {
                throw new CommandException($"Failed to evaluate condition '{cond}': {ex.Message}");
            }
        }
    }
}
