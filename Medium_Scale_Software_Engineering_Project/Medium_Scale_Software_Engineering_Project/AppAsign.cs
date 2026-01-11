using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Implements assignment functionality for variables that have already been declared.
    /// Supported syntax: "set varName = expression" or "varName = expression".
    /// Example usages include "set x = 10" and "x = y + 5".
    /// This command modifies the value of an existing variable without creating a new one.
    /// </summary>
    public class AppAsign : Command, ICommand
    {
        // Identifier of the variable being assigned
        private string VarName;

        // Expression whose evaluated result will be assigned
        private string Expression;

        // Reference to the stored program for variable access and expression evaluation
        private StoredProgram Program;

        /// <summary>
        /// Creates a new instance of the assignment command.
        /// Required for instantiation via the CommandFactory.
        /// </summary>
        public AppAsign() { }

        /// <summary>
        /// Associates the command with the current stored program and parses the assignment input.
        /// Separates the variable name and the expression from the assignment statement.
        /// </summary>
        /// <param name="Program">The stored program currently being executed</param>
        /// <param name="Params">Assignment text in the form "varName = expression"</param>
        /// <exception cref="CommandException">
        /// Thrown when the assignment statement does not follow the expected format
        /// </exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);
            this.Program = Program;

            string[] parts = Params.Split('=');

            if (parts.Length != 2)
                throw new CommandException("Assignment requires format: set variable = expression");

            VarName = parts[0].Trim();
            Expression = parts[1].Trim();
        }

        /// <summary>
        /// Parameter checking is not required for this command.
        /// Method is implemented to satisfy the ICommand interface.
        /// </summary>
        /// <param name="parameterList">Parameter list (not used)</param>
        public override void CheckParameters(string[] parameterList) { }

        /// <summary>
        /// Compilation stage for the assignment command.
        /// No compilation logic is required as validation occurs during execution.
        /// </summary>
        public override void Compile() { }

        /// <summary>
        /// Executes the assignment by evaluating the expression and updating the target variable.
        /// Supports both literal values and computed expressions, with appropriate type validation.
        /// </summary>
        /// <exception cref="CommandException">
        /// Thrown when the target variable does not exist, the expression is invalid,
        /// or the evaluated value cannot be assigned due to a type mismatch
        /// </exception>
        public override void Execute()
        {
            string expr = Expression.Trim();
            string evalResult;

            bool isSimpleLiteral = !Program.IsExpression(expr);

            if (isSimpleLiteral)
            {
                if (Program.VariableExists(expr))
                    evalResult = Program.GetVarValue(expr);
                else
                    evalResult = expr;
            }
            else
            {
                try
                {
                    evalResult = Program.EvaluateExpression(expr);
                }
                catch
                {
                    throw new CommandException($"Invalid assignment expression: '{expr}'");
                }
            }

            evalResult = evalResult.Trim();

            Evaluation target = Program.GetVariable(VarName);

            if (target is AppInt intTarget)
            {
                if (int.TryParse(evalResult, out int val))
                {
                    intTarget.Value = val;
                    System.Diagnostics.Debug.WriteLine($"Assigned {VarName} = {val}");
                }
                else
                    throw new CommandException($"Cannot assign '{evalResult}' to int '{VarName}'");
            }
            else if (target is AppReal realTarget)
            {
                if (double.TryParse(evalResult, out double val))
                {
                    realTarget.RealValue = val;
                    System.Diagnostics.Debug.WriteLine($"Assigned {VarName} = {val}");
                }
                else
                    throw new CommandException($"Cannot assign '{evalResult}' to real '{VarName}'");
            }
            else
            {
                throw new CommandException($"Unsupported variable type for '{VarName}'");
            }
        }
    }
}
