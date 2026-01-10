using BOOSE;
using Medium_Scale_Software_Engineering_Project;
using MYBooseApp;

namespace MYBooseApp
{
    /// <summary>
    /// Represents a custom rectangle drawing command for the MYBooseApp environment.
    /// Extends <see cref="CommandTwoParameters"/> to handle width and height parameters.
    /// </summary>
    internal class AppRect : CommandTwoParameters
    {
        /// <summary>
        /// Executes the rectangle command by parsing the parameters and drawing
        /// a rectangle on the canvas. Ensures parameters are valid integers
        /// and not negative.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown if parameters cannot be parsed to integers, if fewer than 2 parameters
        /// are provided, or if width/height are negative.
        /// </exception>
        public override void Execute()
        {
            try
            {
                // Parse parameters safely
                base.Execute();
            }
            catch (StoredProgramException ex)
            {
                throw new Exception($"Rectangle parameters must be integers. Details: {ex.Message}");
            }

            if (Paramsint.Length < 2)
            {
                throw new Exception("Rectangle command requires exactly 2 integer parameters.");
            }

            int w = Paramsint[0];
            int h = Paramsint[1];

            if (w < 0 || h < 0)
            {
                throw new CommandException($"Rectangle width/height cannot be negative. Received ({w}, {h}).");
            }

            // Draw the rectangle on the canvas (unfilled)
            Canvas.Rect(w, h, filled: false);
        }
    }
}
