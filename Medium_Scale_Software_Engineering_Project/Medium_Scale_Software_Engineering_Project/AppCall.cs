using BOOSE;
using System.Diagnostics;

namespace MYBooseApp
{
    /// <summary>
    /// Provides support for invoking user-defined methods with argument passing
    /// and optional return value handling.
    /// Call syntax: "call methodName [arg1 arg2 ...]".
    /// Example usage: "call mulMethod 10 5".
    /// This command manages parameter binding, return variable setup,
    /// and program counter redirection to simulate a standard function call.
    /// </summary>
    public class AppCall : Command, ICommand
    {
        // Identifier of the method to be invoked
        private string methodName;

        // Collection of argument expressions supplied to the method
        private string[] arguments;

        /// <summary>
        /// Creates a new instance of the AppCall command.
        /// Required for instantiation through the CommandFactory.
        /// </summary>
        public AppCall() { }

        /// <summary>
        /// Associates the command with the active stored program and parses
        /// the method name and argument list from the call statement.
        /// </summary>
        /// <param name="Program">The stored program currently being executed</param>
        /// <param name="Params">Call arguments in the form "methodName arg1 arg2 ..."</param>
        /// <exception cref="CommandException">
        /// Thrown when the call statement does not include a method name
        /// </exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);

            string[] parts = Params.Trim().Split(
                new[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                throw new CommandException("Call requires a method name");

            methodName = parts[0];

            if (parts.Length > 1)
            {
                arguments = new string[parts.Length - 1];
                System.Array.Copy(parts, 1, arguments, 0, parts.Length - 1);
            }
            else
            {
                arguments = new string[0];
            }
        }

        /// <summary>
        /// Parameter validation is not required for the call command.
        /// This method exists to satisfy the ICommand interface.
        /// </summary>
        /// <param name="parameterList">Parameter list (not used)</param>
        public override void CheckParameters(string[] parameterList) { }

        /// <summary>
        /// No compilation logic is required for method calls.
        /// Method existence and argument validation occur at runtime.
        /// </summary>
        public override void Compile() { }

        /// <summary>
        /// Executes the method call by resolving the target method,
        /// creating local parameter variables, preparing a return location,
        /// and transferring control to the method body.
        /// This mirrors the behavior of a function call in conventional languages.
        /// </summary>
        /// <exception cref="BOOSEException">
        /// Thrown when the stored program is not an AppStoredProgram
        /// or when the specified method cannot be found
        /// </exception>
        /// <exception cref="CommandException">
        /// Thrown when the number of supplied arguments does not match
        /// the method's parameter definition
        /// </exception>
        public override void Execute()
        {
            if (program is not AppStoredProgram extProgram)
                throw new BOOSEException("AppCall requires AppStoredProgram.");

            if (!extProgram.MethodExists(methodName))
                throw new BOOSEException($"No such method: {methodName}");

            int methodStartIndex = extProgram.GetMethodStartIndex(methodName);

            var methodCmd = extProgram.GetCommand(methodStartIndex);
            if (methodCmd is not AppMethod actualMethodCmd)
                throw new BOOSEException(
                    $"Method '{methodName}' not found at expected index");

            var parameters = actualMethodCmd.GetParameters();

            if (arguments.Length != parameters.Count)
                throw new CommandException(
                    $"Method '{methodName}' expects {parameters.Count} arguments, got {arguments.Length}");

            for (int i = 0; i < parameters.Count; i++)
            {
                var (paramType, paramName) = parameters[i];
                string argValue = EvaluateExpression(arguments[i]);

                if (paramType == "int")
                {
                    var paramVar = new AppInt();
                    paramVar.Set(program, $"{paramName} = {argValue}");
                    paramVar.VarName = paramName;
                    paramVar.Value = int.Parse(argValue);
                    program.AddVariable(paramVar);
                }
                else if (paramType == "real")
                {
                    var paramVar = new AppReal();
                    paramVar.Set(program, $"{paramName} = {argValue}");
                    paramVar.VarName = paramName;
                    paramVar.RealValue = double.Parse(argValue);
                    program.AddVariable(paramVar);
                }

                Debug.WriteLine(
                    $"Call: Set parameter {paramName} = {argValue}");
            }

            string returnType = actualMethodCmd.GetReturnType();
            if (!string.IsNullOrEmpty(returnType))
            {
                if (returnType == "int")
                {
                    var returnVar = new AppInt();
                    returnVar.Set(program, $"{methodName} = 0");
                    returnVar.VarName = methodName;
                    returnVar.Value = 0;

                    if (!program.VariableExists(methodName))
                        program.AddVariable(returnVar);
                }
                else if (returnType == "real")
                {
                    var returnVar = new AppReal();
                    returnVar.Set(program, $"{methodName} = 0");
                    returnVar.VarName = methodName;
                    returnVar.RealValue = 0.0;

                    if (!program.VariableExists(methodName))
                        program.AddVariable(returnVar);
                }
            }

            int returnAddress = program.PC;

            for (int i = methodStartIndex + 1; i < extProgram.CommandCount; i++)
            {
                var cmd = extProgram.GetCommand(i);
                if (cmd is AppEndMethod endMethodCmd)
                {
                    endMethodCmd.SetReturnAddress(returnAddress);
                    break;
                }
            }

            Debug.WriteLine(
                $"Call '{methodName}' at PC={program.PC - 1} → jumping to method at PC={methodStartIndex + 1}");

            program.PC = methodStartIndex + 1;
        }

        /// <summary>
        /// Evaluates an argument expression by substituting variable references
        /// with their current values prior to evaluation.
        /// Returns the final evaluated result as a string.
        /// </summary>
        /// <param name="expr">
        /// The argument expression (e.g., "10", "x + 5", "variableName")
        /// </param>
        /// <returns>The evaluated expression result</returns>
        private string EvaluateExpression(string expr)
        {
            expr = expr.Trim();

            expr = System.Text.RegularExpressions.Regex.Replace(
                expr,
                @"\b[a-zA-Z_][a-zA-Z0-9_]*\b",
                match =>
                {
                    string varName = match.Value;
                    if (program.VariableExists(varName))
                        return program.GetVarValue(varName);
                    return varName;
                });

            if (int.TryParse(expr, out _) || double.TryParse(expr, out _))
                return expr;

            try
            {
                return program.EvaluateExpression(expr);
            }
            catch
            {
                return expr;
            }
        }
    }
}
