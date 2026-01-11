using BOOSE;
using System;

namespace MYBooseApp
{
    /// <summary>
    /// Defines a one-dimensional array capable of storing integer or real values.
    /// Declaration syntax: array int|real arrayName size
    /// </summary>
    public class AppArray : Evaluation, ICommand
    {
        protected string type;
        protected int size;
        protected int[] intArray;
        protected double[] realArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppArray"/> class.
        /// </summary>
        public AppArray() { }

        /// <summary>
        /// Returns the total number of elements in the array.
        /// </summary>
        public int Size => size;

        /// <summary>
        /// Extracts and assigns the array declaration details from the command parameters.
        /// </summary>
        /// <param name="Program">The stored program this array belongs to</param>
        /// <param name="Params">The argument string following the array keyword</param>
        public new void Set(StoredProgram Program, string Params)
        {
            base.Set(Program, Params);
            this.Program = Program;

            string[] parts = Params.Trim().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3)
                throw new CommandException("Array requires: type name size");

            type = parts[0].ToLower(); // int or real
            VarName = parts[1];
            size = int.Parse(parts[2]);
        }

        /// <summary>
        /// Parameter validation is not required for this command.
        /// </summary>
        public override void CheckParameters(string[] parameterList) { }

        /// <summary>
        /// Allocates memory for the array and registers it within the program.
        /// </summary>
        public override void Compile()
        {
            if (Program == null || string.IsNullOrEmpty(VarName))
                throw new CommandException("Invalid array declaration");

            if (!Program.VariableExists(VarName))
            {
                if (type == "int")
                    intArray = new int[size];
                else if (type == "real")
                    realArray = new double[size];
                else
                    throw new CommandException($"Invalid array type: {type}");

                Program.AddVariable(this);
            }
        }

        /// <summary>
        /// Arrays do not require runtime execution.
        /// </summary>
        public override void Execute() { }

        /// <summary>
        /// Retrieves an integer element from the array at the specified index.
        /// </summary>
        public int GetIntArray(int index)
        {
            if (intArray == null) throw new CommandException($"{VarName} is not an int array");
            if (index < 0 || index >= size) throw new CommandException($"Array index out of bounds: [{index}]");
            return intArray[index];
        }

        /// <summary>
        /// Retrieves a real (double) element from the array at the specified index.
        /// </summary>
        public double GetRealArray(int index)
        {
            if (realArray == null) throw new CommandException($"{VarName} is not a real array");
            if (index < 0 || index >= size) throw new CommandException($"Array index out of bounds: [{index}]");
            return realArray[index];
        }

        /// <summary>
        /// Assigns an integer value to the array at the given index.
        /// </summary>
        public void SetIntArray(int val, int index)
        {
            if (intArray == null) throw new CommandException($"{VarName} is not an int array");
            if (index < 0 || index >= size) throw new CommandException($"Array index out of bounds: [{index}]");
            intArray[index] = val;
        }

        /// <summary>
        /// Assigns a real (double) value to the array at the given index.
        /// </summary>
        public void SetRealArray(double val, int index)
        {
            if (realArray == null) throw new CommandException($"{VarName} is not a real array");
            if (index < 0 || index >= size) throw new CommandException($"Array index out of bounds: [{index}]");
            realArray[index] = val;
        }

        /// <summary>
        /// Indicates whether the array stores integer values.
        /// </summary>
        public bool IsIntArray() => type == "int";

        /// <summary>
        /// Indicates whether the array stores real (double) values.
        /// </summary>
        public bool IsRealArray() => type == "real";
    }
}
