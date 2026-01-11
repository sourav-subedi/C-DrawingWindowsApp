using BOOSE;
using System;

namespace MYBooseApp
{
    /// <summary>
    /// Implements the 'peek' command for reading a value from a 1D array into a variable.
    /// Syntax: peek targetVar = arrayName index
    /// Supports both integer and real arrays.
    /// </summary>
    public class AppPeek : Command, ICommand
    {
        private string targetVar;
        private string arrayName;
        private string indexExpr;

        /// <summary>
        /// Default constructor required by CommandFactory.
        /// </summary>
        public AppPeek() { }

        /// <summary>
        /// Parses the peek command parameters.
        /// Expects format: targetVar = arrayName index
        /// </summary>
        /// <param name="Program">Reference to the stored program</param>
        /// <param name="Params">Command string containing target variable, array name, and index</param>
        /// <exception cref="CommandException">Thrown if the format is invalid</exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);
            this.Program = Program;

            if (!Params.Contains("="))
                throw new CommandException("Peek requires assignment: peek targetVar = arrayName index");

            int eq = Params.IndexOf('=');
            targetVar = Params.Substring(0, eq).Trim();
            string rightSide = Params.Substring(eq + 1).Trim();

            string[] parts = rightSide.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                throw new CommandException("Peek requires format: targetVar = arrayName index");

            arrayName = parts[0];
            indexExpr = parts[1];
        }

        /// <summary>
        /// No additional parameter validation required.
        /// </summary>
        /// <param name="parameter">Parameter array (not used)</param>
        public override void CheckParameters(string[] parameter) { }

        /// <summary>
        /// Compilation phase: Validates that the array and target variable exist and types are compatible.
        /// </summary>
        /// <exception cref="CommandException">Thrown if variables are missing or types mismatch</exception>
        public override void Compile()
        {
            if (!Program.VariableExists(arrayName))
                throw new CommandException($"Array '{arrayName}' not declared");

            if (!Program.VariableExists(targetVar))
                throw new CommandException($"Target variable '{targetVar}' not declared");

            if (!(Program.GetVariable(arrayName) is AppArray))
                throw new CommandException($"'{arrayName}' is not an array");
        }

        /// <summary>
        /// Execution phase: Evaluates the index and reads the value from the array into the target variable.
        /// </summary>
        /// <exception cref="CommandException">
        /// Thrown if the index cannot be evaluated to an integer or there is a type mismatch.
        /// </exception>
        public override void Execute()
        {
            AppArray array = Program.GetVariable(arrayName) as AppArray;
            Evaluation target = Program.GetVariable(targetVar);

            int index;
            if (!int.TryParse(indexExpr, out index))
            {
                string evalResult = Program.EvaluateExpression(indexExpr);
                if (!int.TryParse(evalResult, out index))
                    throw new CommandException($"Index '{indexExpr}' evaluated to invalid integer");
            }

            if (array.IsIntArray() && target is AppInt iTarget)
                iTarget.Value = array.GetIntArray(index);
            else if (array.IsRealArray() && target is AppReal rTarget)
                rTarget.RealValue = array.GetRealArray(index);
            else
                throw new CommandException("Type mismatch in peek operation");
        }
    }
}
