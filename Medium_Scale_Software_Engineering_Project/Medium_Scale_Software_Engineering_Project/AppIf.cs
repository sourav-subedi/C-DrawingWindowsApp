using BOOSE;
using System;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MYBooseApp
{
    /// <summary>
    /// Represents an 'if' conditional command in BOOSE.
    /// Evaluates a boolean condition and controls execution flow accordingly.
    /// Supports nested if-else structures when used with <see cref="AppStoredProgram"/>.
    /// </summary>
    public class AppIf : Command, ICommand
    {
        /// <summary>
        /// Stores the condition expression provided by the user.
        /// </summary>
        private string condition;

        /// <summary>
        /// Default constructor for instantiation via <see cref="AppCommandFactory"/>.
        /// </summary>
        public AppIf() { }

        /// <summary>
        /// Initializes the command with the stored program and captures the condition expression.
        /// </summary>
        /// <param name="Program">The current <see cref="StoredProgram"/> instance.</param>
        /// <param name="Params">The condition string, e.g., "x > 5".</param>
        /// <exception cref="CommandException">Thrown if the condition is empty or null.</exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);
            condition = Params?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(condition))
                throw new CommandException("If command requires a condition expression.");
        }

        /// <summary>
        /// No additional parameter validation required beyond <see cref="Set"/>.
        /// </summary>
        /// <param name="parameterList">Array of parameters (not used).</param>
        public override void CheckParameters(string[] parameterList) { }

        /// <summary>
        /// Compilation phase does not require special actions for <c>if</c>.
        /// </summary>
        public override void Compile() { }

        /// <summary>
        /// Executes the 'if' command by evaluating the condition.
        /// If true, execution continues normally.
        /// If false, jumps to the matching <see cref="AppElse"/> (if present) or <see cref="AppEndIf"/>.
        /// </summary>
        /// <exception cref="BOOSEException">Thrown if no matching end-if is found.</exception>
        public override void Execute()
        {
            if (program is not AppStoredProgram extProgram)
                throw new BOOSEException("AppIf requires AppStoredProgram for control flow.");

            bool isTrue = EvaluateCondition(condition);

            Debug.WriteLine($"If at PC={program.PC}: condition '{condition}' → {isTrue}");

            if (isTrue)
            {
                Debug.WriteLine("Condition true → continue normally");
                return;
            }

            Debug.WriteLine("Condition false → jumping forward...");

            int myPosition = program.PC - 1;
            int nestingLevel = 0;

            for (int i = myPosition + 1; i < extProgram.CommandCount; i++)
            {
                var cmd = extProgram.GetCommand(i);

                if (cmd is AppIf)
                    nestingLevel++;
                else if (cmd is AppEndIf)
                {
                    if (nestingLevel == 0)
                    {
                        Debug.WriteLine($"Jumping to end if at PC={i}");
                        program.PC = i;
                        return;
                    }
                    nestingLevel--;
                }
                else if (cmd is AppElse && nestingLevel == 0)
                {
                    Debug.WriteLine($"Jumping to else block (after marker) at PC={i + 1}");
                    program.PC = i + 1;
                    return;
                }
            }

            throw new BOOSEException("No matching 'end if' found for this 'if' statement.");
        }

        /// <summary>
        /// Evaluates the boolean condition string using variable substitution.
        /// </summary>
        /// <param name="cond">The raw condition expression.</param>
        /// <returns>True if the condition evaluates to true, otherwise false.</returns>
        /// <exception cref="CommandException">Thrown if evaluation fails.</exception>
        private bool EvaluateCondition(string cond)
        {
            string expr = cond.Trim();

            // Replace variable names with current values
            expr = Regex.Replace(expr, @"\b[a-zA-Z_][a-zA-Z0-9_]*\b", match =>
            {
                string varName = match.Value;
                if (program.VariableExists(varName))
                    return program.GetVarValue(varName);
                return varName;
            });

            // Convert logical operators to DataTable-compatible syntax
            expr = expr.Replace("&&", " AND ")
                       .Replace("||", " OR ")
                       .Replace("!", "NOT ");

            try
            {
                var dt = new DataTable();
                object result = dt.Compute(expr, "");
                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                throw new CommandException($"Failed to evaluate condition '{cond}': {ex.Message}");
            }
        }
    }
}
