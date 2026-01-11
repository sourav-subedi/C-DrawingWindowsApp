using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest.Shapes;

/// <summary>
/// Unit tests for the <see cref="AppDrawTo"/> command using <see cref="TestAppCanvas"/> for thread-safe testing.
/// These tests verify proper handling of X/Y coordinates with different separators, expressions, and invalid inputs.
/// </summary>
[TestClass]
public class DrawToUnitTest
{
    /// <summary>
    /// Verifies that <see cref="AppDrawTo"/> executes successfully with valid literal integers separated by space.
    /// </summary>
    [TestMethod]
    public void DrawTo_ValidLiteralsWithSpaceSeparator_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppDrawTo(canvas);

        command.Set(new StoredProgram(canvas), "120 180");
        command.CheckParameters(new[] { "120", "180" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that <see cref="AppDrawTo"/> executes correctly with literals separated by commas and spaces.
    /// </summary>
    [TestMethod]
    public void DrawTo_ValidLiteralsWithCommaAndSpaces_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppDrawTo(canvas);

        command.Set(new StoredProgram(canvas), "  45 ,  90  ");
        command.CheckParameters(new[] { "45", "90" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that <see cref="AppDrawTo"/> executes successfully with simple expressions that contain no spaces.
    /// </summary>
    [TestMethod]
    public void DrawTo_SimpleExpressionNoSpaces_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var program = new StoredProgram(canvas);
        var command = new AppDrawTo(canvas);

        command.Set(program, "50+30 100-25");
        command.CheckParameters(new[] { "50+30", "100-25" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that <see cref="AppDrawTo"/> throws a <see cref="CommandException"/> when the Y parameter is missing.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void DrawTo_MissingYParameter_ThrowsException()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppDrawTo(canvas);
            command.Set(new StoredProgram(canvas), "150");
            command.CheckParameters(new[] { "150" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }

    /// <summary>
    /// Verifies that <see cref="AppDrawTo"/> throws a <see cref="CommandException"/> when X is invalid/non-numeric.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void DrawTo_InvalidXValue_ThrowsException()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppDrawTo(canvas);
            command.Set(new StoredProgram(canvas), "hello 120");
            command.CheckParameters(new[] { "hello", "120" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }

    /// <summary>
    /// Verifies that <see cref="AppDrawTo"/> fails with expressions containing spaces.
    /// This test documents a current limitation of the Set() split logic.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void DrawTo_ExpressionWithSpaces_ThrowsAsExpected()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppDrawTo(canvas);
            command.Set(new StoredProgram(canvas), "100 + 50 200 - 30");
            // Splits into more than 2 parts → CheckParameters should throw
            command.CheckParameters(new[] { "100", "+", "50", "200", "-", "30" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }
}
