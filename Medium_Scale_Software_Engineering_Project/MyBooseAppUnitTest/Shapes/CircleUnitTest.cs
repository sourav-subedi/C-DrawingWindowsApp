using MYBooseApp;
using BOOSE;

namespace UnitTests.Shapes;

/// <summary>
/// Circle command unit tests using TestAppCanvas for thread-safe testing
/// </summary>
[TestClass]
public class CircleUnitTest
{
    // 1. Valid radius, no filled flag
    /// <summary>
    /// Commands Circle with valid radius and checks for successful execution
    /// </summary>
    [TestMethod]
    public void Circle_ValidRadius_ExecutesSuccessfully()
    {
        // Create independent TestAppCanvas for this test
        var canvas = new TestAppCanvas(200, 200);
        AppCircle command = new AppCircle(canvas);
        command.Set(new StoredProgram(canvas), "50");
        command.CheckParameters(new[] { "50" });
        command.Compile();
        command.Execute();
        canvas.Dispose(); // Clean up resources
    }

    // 2. Valid radius, filled = true
    /// <summary>
    /// Commands Circle with valid radius and filled flag set to true, checks for successful execution
    /// </summary>
    [TestMethod]
    public void Circle_FilledTrue_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(200, 200);
        AppCircle command = new AppCircle(canvas);
        command.Set(new StoredProgram(canvas), "30,true"); 
        command.CheckParameters(new[] { "30", "true" });
        command.Compile();
        command.Execute();
        canvas.Dispose();
    }

    // 3. Valid radius, filled = false explicitly
    /// <summary>
    /// Commands Circle with valid radius and filled flag set to false, checks for successful execution
    /// </summary>
    [TestMethod]
    public void Circle_FilledFalse_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(200, 200);
        AppCircle command = new AppCircle(canvas);
        command.Set(new StoredProgram(canvas), "25,false"); 
        command.CheckParameters(new[] { "25", "false" });
        command.Compile();
        command.Execute();
        canvas.Dispose();
    }

    // 4. Radius = 0 → should throw CommandException
    /// <summary>
    /// Commands Circle with radius zero and expects a CommandException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void Circle_InvalidRadiusZero_ThrowsException()
    {
        var canvas = new TestAppCanvas(200, 200);
        try
        {
            AppCircle command = new AppCircle(canvas);
            command.Set(new StoredProgram(canvas), "0");
            command.CheckParameters(new[] { "0" });
            command.Compile();
            command.Execute(); // Should throw
        }
        finally
        {
            canvas.Dispose();
        }
    }

    // 5. Radius < 0 → should throw CommandException
    /// <summary>
    /// Commands Circle with negative radius and expects a CommandException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void Circle_InvalidRadiusNegative_ThrowsException()
    {
        var canvas = new TestAppCanvas(200, 200);
        try
        {
            AppCircle command = new AppCircle(canvas);
            command.Set(new StoredProgram(canvas), "-10");
            command.CheckParameters(new[] { "-10" });
            command.Compile();
            command.Execute(); // Should throw
        }
        finally
        {
            canvas.Dispose();
        }
    }

    // 6. Invalid filled flag → should throw CommandException
    /// <summary>
    /// Commands Circle with invalid filled flag and expects a CommandException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void Circle_InvalidFilledFlag_ThrowsException()
    {
        var canvas = new TestAppCanvas(200, 200);
        try
        {
            AppCircle command = new AppCircle(canvas);
            command.Set(new StoredProgram(canvas), "40,wrongflag"); 
            command.CheckParameters(new[] { "40", "wrongflag" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }
}