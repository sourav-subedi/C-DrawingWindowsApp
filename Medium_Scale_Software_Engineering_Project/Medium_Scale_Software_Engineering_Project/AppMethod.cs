using BOOSE;
using System.Diagnostics;
using System.Collections.Generic;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom method declaration in BOOSE.
    /// Handles parsing of return type, method name, parameters, and supports control flow management.
    /// Example syntax: "method int mulMethod int one, int two"
    /// </summary>
    public class AppMethod : CompoundCommand, ICommand
    {
        /// <summary>
        /// The return type of the method ("int", "real", or empty for void)
        /// </summary>
        private string returnType;

        /// <summary>
        /// The name of the method
        /// </summary>
        private string methodName;

        /// <summary>
        /// List of parameters as tuples of (type, name)
        /// </summary>
        private List<(string type, string name)> parameters = new List<(string, string)>();

        /// <summary>
        /// Index of the method declaration in the program
        /// </summary>
        private int methodStartIndex = -1;

        /// <summary>
        /// Index of the corresponding end of the method
        /// </summary>
        private int methodEndIndex = -1;

        /// <summary>
        /// Default constructor for CommandFactory
        /// </summary>
        public AppMethod() { }

        /// <summary>
        /// Initializes the method declaration by parsing the provided string.
        /// Extracts optional return type, required method name, and optional parameters.
        /// </summary>
        /// <param name="Program">The current StoredProgram</param>
        /// <param name="Params">Method declaration string in the format "[returnType] methodName [type param1, type param2, ...]"</param>
        /// <exception cref="CommandException">Thrown if method name is missing or parameters are invalid</exception>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);

            string[] parts = Params.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                throw new CommandException("Method requires at least a method name");

            int currentIndex = 0;

            // Optional return type
            if (parts[0] == "int" || parts[0] == "real")
            {
                returnType = parts[0];
                currentIndex++;
            }

            if (currentIndex >= parts.Length)
                throw new CommandException("Method requires a method name");

            methodName = parts[currentIndex++];

            if (currentIndex < parts.Length)
            {
                string paramString = string.Join(" ", parts, currentIndex, parts.Length - currentIndex);
                ParseParameters(paramString);
            }
        }

        /// <summary>
        /// Parses the parameter string into typed variables.
        /// </summary>
        /// <param name="paramString">Parameter string in the format "type name, type name, ..."</param>
        /// <exception cref="CommandException">Thrown if any parameter is incorrectly formatted</exception>
        private void ParseParameters(string paramString)
        {
            string[] paramParts = paramString.Split(',');

            foreach (string part in paramParts)
            {
                string[] tokens = part.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length == 2)
                {
                    string paramType = tokens[0];
                    string paramName = tokens[1];

                    if (paramType != "int" && paramType != "real")
                        throw new CommandException($"Invalid parameter type: {paramType}");

                    parameters.Add((paramType, paramName));
                }
                else if (tokens.Length > 0)
                {
                    throw new CommandException($"Invalid parameter format: {part}");
                }
            }
        }

        /// <summary>
        /// Parameter validation (not used for methods)
        /// </summary>
        /// <param name="parameterList">Array of parameters (not used)</param>
        public override void CheckParameters(string[] parameterList) { }

        /// <summary>
        /// Compilation phase: records position, registers method in AppStoredProgram, and pushes to stack
        /// </summary>
        /// <exception cref="CommandException">Thrown if AppStoredProgram is not used</exception>
        public override void Compile()
        {
            methodStartIndex = program.Count - 1;

            Debug.WriteLine($"Method '{methodName}' found at index {methodStartIndex}");

            if (program is AppStoredProgram extProgram)
            {
                extProgram.RegisterMethod(methodName, methodStartIndex);
            }
            else
            {
                throw new CommandException("MethodCommand requires AppStoredProgram.");
            }

            program.Push(this);
        }

        /// <summary>
        /// Sets the index of the corresponding end of the method
        /// </summary>
        /// <param name="index">Program index of AppEndMethod</param>
        public void SetEndMethodIndex(int index)
        {
            methodEndIndex = index;
        }

        /// <summary>
        /// Returns the method's starting position in the program
        /// </summary>
        public int GetMethodStartIndex() => methodStartIndex;

        /// <summary>
        /// Returns the method's name
        /// </summary>
        public string GetMethodName() => methodName;

        /// <summary>
        /// Returns the return type of the method
        /// </summary>
        public string GetReturnType() => returnType;

        /// <summary>
        /// Returns the method's parameter list
        /// </summary>
        public List<(string type, string name)> GetParameters() => parameters;

        /// <summary>
        /// Execution phase: skips method body if reached directly; should only execute via AppCall
        /// </summary>
        /// <exception cref="BOOSEException">Thrown if end position is not set</exception>
        public override void Execute()
        {
            if (methodEndIndex == -1)
                throw new BOOSEException("Method end position not set during compilation.");

            Debug.WriteLine($"Method '{methodName}' at PC={program.PC - 1} → skipping to PC={methodEndIndex + 1}");

            program.PC = methodEndIndex + 1;
        }
    }
}
