using BOOSE;
using Medium_Scale_Software_Engineering_Project;
using MYBooseApp;

internal class AppRect : CommandTwoParameters
{
    public override void Execute()
    {
        // --- Debug lines ---
        //System.Diagnostics.Debug.WriteLine("Width variable exists: " + Program.VariableExists("width"));
        //System.Diagnostics.Debug.WriteLine("Width value: " + ((AppInt)Program.GetVariable("width"))?.Value);
        //System.Diagnostics.Debug.WriteLine("Height variable exists: " + Program.VariableExists("height"));
        //System.Diagnostics.Debug.WriteLine("Height value: " + ((AppInt)Program.GetVariable("height"))?.Value);

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

        // Draw the rectangle
        Canvas.Rect(w, h, filled: false);
    }
}
