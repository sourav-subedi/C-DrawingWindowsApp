using BOOSE;
using System;

namespace MYBooseApp
{
    /// <summary>
    /// Overrides the base class to remove restriction and add proper validation
    /// </summary>
    internal class AppRect : CommandTwoParameters
    {
        public override void Execute()
        {
            try
            {
                // Parse parameters safely
                base.Execute();
            }
            catch (StoredProgramException ex)
            {
                // Message if parameter can't be evaluated
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

            // Draw the rectangle with validated parameters
            Canvas.Rect(w, h, filled: false);
        }
    }
}
