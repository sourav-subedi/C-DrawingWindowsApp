using BOOSE;
using System;

namespace MYBooseApp
{
    /// <summary>
    /// Override base moveto class to add validation for positive integers
    /// and handle invalid expressions gracefully
    /// </summary>
    public class AppMoveto : CommandTwoParameters
    {
        public override void Execute()
        {
            try
            {
                // Try base execution to parse parameters
                base.Execute();
            }
            catch (StoredProgramException ex)
            {
                // Catch parsing errors and provide the message
                throw new Exception($"MoveTo parameters must be integers. Details: {ex.Message}");
            }

            // Validate that both parameters are positive integers
            if (Paramsint.Length < 2)
            {
                throw new Exception("MoveTo command requires exactly 2 integer parameters.");
            }

            int x = Paramsint[0];
            int y = Paramsint[1];

            if (x < 0 || y < 0)
            {
                throw new Exception($"MoveTo parameters must be positive integers. Received ({x}, {y}).");
            }

            // Execute the actual movement
            Canvas.MoveTo(x, y);
        }
    }
}
