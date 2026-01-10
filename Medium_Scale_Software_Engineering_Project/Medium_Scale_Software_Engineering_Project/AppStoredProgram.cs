using BOOSE;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a stored program in the MYBooseApp environment.
    /// Handles AppInt, AppReal, and AppBoolean variable types and executes commands on a canvas.
    /// </summary>
    public class AppStoredProgram : StoredProgram
    {
        private ICanvas canvas;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppStoredProgram"/> class with a canvas.
        /// </summary>
        /// <param name="canvas">The canvas used for drawing and output.</param>
        public AppStoredProgram(ICanvas canvas) : base(canvas)
        {
            this.canvas = canvas;
        }

        /// <summary>
        /// Gets the canvas associated with this stored program.
        /// </summary>
        /// <returns>The <see cref="ICanvas"/> instance.</returns>
        public ICanvas GetCanvas()
        {
            return canvas;
        }

        /// <summary>
        /// Updates the value of a variable of type double.
        /// Handles <see cref="AppReal"/> and <see cref="Real"/>.
        /// </summary>
        /// <param name="varName">The variable name.</param>
        /// <param name="value">The double value to assign.</param>
        /// <exception cref="CommandException">Thrown if the variable type is not real.</exception>
        public override void UpdateVariable(string varName, double value)
        {
            Evaluation variable = (Evaluation)GetVariable(varName);

            if (variable is Real r)
            {
                r.Value = value;
            }
            else if (variable is AppReal ar)
            {
                ar.Value = value;
            }
            else
            {
                throw new CommandException("Type mismatch, expected a real value");
            }
        }

        /// <summary>
        /// Updates the value of a variable of type integer.
        /// Handles <see cref="AppInt"/> and <see cref="Int"/>.
        /// </summary>
        /// <param name="varName">The variable name.</param>
        /// <param name="value">The integer value to assign.</param>
        public override void UpdateVariable(string varName, int value)
        {
            int index = FindVariable(varName);
            Evaluation evaluation = (Evaluation)GetVariable(index);

            if (evaluation is Int i)
            {
                i.Value = value;
            }
            else if (evaluation is AppInt ai)
            {
                ai.Value = value;
            }
            else
            {
                evaluation.Value = value;
            }
        }

        /// <summary>
        /// Updates the value of a variable of type boolean.
        /// Handles <see cref="AppBoolean"/> and <see cref="BOOSE.Boolean"/>.
        /// </summary>
        /// <param name="varName">The variable name.</param>
        /// <param name="value">The boolean value to assign.</param>
        /// <exception cref="CommandException">Thrown if the variable type is not boolean.</exception>
        public override void UpdateVariable(string varName, bool value)
        {
            Evaluation variable = GetVariable(varName);

            if (variable is BOOSE.Boolean b)
            {
                b.BoolValue = value;
            }
            else if (variable is AppBoolean ab)
            {
                ab.BoolValue = value;
            }
            else
            {
                throw new CommandException("Type mismatch, expected a boolean value");
            }
        }

        /// <summary>
        /// Gets the string representation of a variable's value.
        /// Supports AppReal, AppBoolean, Real, and BOOSE.Boolean.
        /// </summary>
        /// <param name="varName">The variable name.</param>
        /// <returns>The value of the variable as a string.</returns>
        /// <exception cref="StoredProgramException">Thrown if the variable is not found.</exception>
        public override string GetVarValue(string varName)
        {
            int num = FindVariable(varName);
            if (num == -1)
            {
                throw new StoredProgramException("Variable not found");
            }

            Evaluation evaluation = GetVariable(num);

            if (evaluation is Real r)
            {
                return r.Value.ToString();
            }
            else if (evaluation is AppReal ar)
            {
                return ar.Value.ToString();
            }
            else if (evaluation is AppBoolean ab)
            {
                return ab.BoolValue ? "true" : "false";
            }
            else if (evaluation is BOOSE.Boolean b)
            {
                return b.BoolValue ? "true" : "false";
            }

            return evaluation.Value.ToString();
        }

        /// <summary>
        /// Executes all commands in the stored program sequentially.
        /// Throws <see cref="StoredProgramException"/> if a runtime error occurs.
        /// </summary>
        /// <exception cref="StoredProgramException">Thrown if a runtime error occurs during execution.</exception>
        public override void Run()
        {
            int num = 0;
            string error = "";
            bool hadRuntimeError = false;

            while (Commandsleft())
            {
                ICommand command = (ICommand)NextCommand();

                if (command == null)
                    continue;

                try
                {
                    num++;
                    command.Execute();
                }
                catch (BOOSEException e)
                {
                    hadRuntimeError = true;
                    error += "Runtime error: " + e.Message + " at line " + PC + Environment.NewLine;
                    break;
                }
            }

            if (hadRuntimeError)
            {
                throw new StoredProgramException(error.Trim());
            }
        }
    }
}
