using MYBooseApp;
using BOOSE;

namespace MyBooseAppUnitTest.Shapes;

/// <summary>
/// Unit tests for the <see cref="AppCircle"/> command using <see cref="TestAppCanvas"/> for thread-safe testing.
/// These tests verify correct handling of radius and filled parameters, including valid and invalid inputs.
/// </summary>
[TestClass]
public class CircleUnitTest
{
    /// <summary>
    /// Verifies that a circle with a valid radius and no filled flag executes successfully.
    /// </summary>
    [TestMethod]
    public void Circle_ValidRadius_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(200, 200);
        AppCircle command = new AppCircle(canvas);
        command.Set(new StoredProgram(canvas), "50");
        command.CheckParameters(new[] { "50" });
        command.Compile();
        command.Execute();
        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that a circle with a valid radius and <c>filled = true</c> executes successfully.
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

    /// <summary>
    /// Verifies that a circle with a valid radius and <c>filled = false</c> executes successfully.
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

    /// <summary>
    /// Verifies that a circle with radius zero throws a <see cref="CommandException"/>.
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
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }

    /// <summary>
    /// Verifies that a circle with a negative radius throws a <see cref="CommandException"/>.
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
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }

    /// <summary>
    /// Verifies that a circle with an invalid filled flag throws a <see cref="CommandException"/>.
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
