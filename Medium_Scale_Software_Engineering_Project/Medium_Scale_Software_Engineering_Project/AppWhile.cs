using BOOSE;
using System;
using System.Text.RegularExpressions;
using System.Data;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom While loop command for the MYBooseApp environment.
    /// Evaluates a condition and loops until the condition is false.
    /// </summary>
    public class AppWhile : CompoundCommand
    {
        /// <summary>
        /// The raw condition expression for this While command.
        /// Must be set before Execute() is called.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Default constructor sets the conditional type.
        /// </summary>
        public AppWhile()
        {
            CondType = ConditionalCommand.conditionalTypes.commWhile;
        }

        /// <summary>
        /// Sets the While loop condition from parser.
        /// </summary>
        public new void Set(StoredProgram program, string parameters)
        {
            base.Set(program, parameters);

            if (string.IsNullOrWhiteSpace(parameters))
                throw new BOOSEException("While loop requires a condition expression.");

            Expression = parameters.Trim();
        }

        /// <summary>
        /// Compiles the While command by pushing itself to the program stack
        /// for linking with AppEnd.
        /// </summary>
        public override void Compile()
        {
            base.Program.Push(this); // so AppEnd can link to it
        }

        /// <summary>
        /// Executes the While loop by evaluating the condition.
        /// If false, jumps to corresponding End using LineNumber.
        /// </summary>
        public override void Execute()
        {
            if (string.IsNullOrWhiteSpace(Expression))
                throw new BOOSEException("While condition expression is not set.");

            bool conditionMet = EvaluateCondition(Expression);

            // Keep base property for compatibility
            if (this is ConditionalCommand cc)
                cc.Condition = conditionMet;

            // If condition is false, jump to End
            if (!conditionMet && CorrespondingCommand != null)
            {
                Program.PC = CorrespondingCommand.LineNumber;
            }
        }

        /// <summary>
        /// Evaluates the condition string dynamically.
        /// Supports variables and logical operators.
        /// </summary>
        private bool EvaluateCondition(string cond)
        {
            if (string.IsNullOrWhiteSpace(cond))
                throw new BOOSEException("Cannot evaluate empty condition.");

            string expr = cond.Trim();

            // Replace variables with their current values
            expr = Regex.Replace(expr, @"\b[a-zA-Z_][a-zA-Z0-9_]*\b", match =>
            {
                string varName = match.Value;
                return Program.VariableExists(varName) ? Program.GetVarValue(varName) : varName;
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
                throw new BOOSEException($"Failed to evaluate condition '{cond}': {ex.Message}");
            }
        }
    }
}
