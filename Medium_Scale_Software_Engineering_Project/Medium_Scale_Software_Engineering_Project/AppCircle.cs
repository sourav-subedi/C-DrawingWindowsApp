using BOOSE;
using System;

namespace MYBooseApp
{
    /// <summary>
    /// The AppCircle class is created with validation for positive radius
    /// </summary>
    internal class AppCircle : CommandOneParameter
    {
        public AppCircle() : base() { }

        public override void Execute()
        {
            try
            {
                // Parse the parameter safely
                base.Execute();
            }
            catch (StoredProgramException ex)
            {
                // Message if parameter can't be evaluated
                throw new Exception($"Circle parameter must be an integer. Details: {ex.Message}");
            }

            if (Paramsint.Length < 1)
            {
                throw new Exception("Circle command requires exactly 1 integer parameter.");
            }

            int radius = Paramsint[0];

            if (radius <= 0)
            {
                throw new Exception($"Circle radius must be a positive integer. Received {radius}.");
            }

            // Draw the circle
            Canvas.Circle(radius, filled: false);
        }
    }
}
