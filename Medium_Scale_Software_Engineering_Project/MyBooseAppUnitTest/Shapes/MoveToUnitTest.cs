using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest.Shapes;

/// <summary>
/// Unit tests for the <see cref="AppMoveTo"/> command using <see cref="TestAppCanvas"/> for thread-safe testing.
/// These tests verify proper handling of X/Y coordinates with different separators, expressions, and invalid inputs.
/// </summary>
[TestClass]
public class MoveToUnitTest
{
    /// <summary>
    /// Verifies that <see cref="AppMoveTo"/> executes successfully with valid literal integers separated by space.
    /// </summary>
    [TestMethod]
    public void MoveTo_ValidLiteralsWithSpaceSeparator_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppMoveTo(canvas);

        command.Set(new StoredProgram(canvas), "150 220");
        command.CheckParameters(new[] { "150", "220" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that <see cref="AppMoveTo"/> executes correctly with literals separated by commas and spaces.
    /// </summary>
    [TestMethod]
    public void MoveTo_ValidLiteralsWithCommaAndSpaces_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppMoveTo(canvas);

        command.Set(new StoredProgram(canvas), " 80 , 120 ");
        command.CheckParameters(new[] { "80", "120" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that <see cref="AppMoveTo"/> executes successfully with simple expressions that contain no spaces.
    /// </summary>
    [TestMethod]
    public void MoveTo_SimpleExpressionNoSpaces_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var program = new StoredProgram(canvas);
        var command = new AppMoveTo(canvas);

        command.Set(program, "50+30 100-25");
        command.CheckParameters(new[] { "50+30", "100-25" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that <see cref="AppMoveTo"/> throws a <see cref="CommandException"/> when the Y parameter is missing.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void MoveTo_MissingYParameter_ThrowsException()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppMoveTo(canvas);
            command.Set(new StoredProgram(canvas), "200");
            command.CheckParameters(new[] { "200" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }

    /// <summary>
    /// Verifies that <see cref="AppMoveTo"/> throws a <see cref="CommandException"/> when X is invalid/non-numeric.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void MoveTo_InvalidXValue_ThrowsException()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppMoveTo(canvas);
            command.Set(new StoredProgram(canvas), "abc 90");
            command.CheckParameters(new[] { "abc", "90" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }

    /// <summary>
    /// Verifies that <see cref="AppDrawTo"/> fails when expressions contain spaces.
    /// This documents a current limitation of the Set() split logic.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void DrawTo_ExpressionWithSpaces_ThrowsAsExpected()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppDrawTo(canvas);

        // Input with spaces → split into too many pieces
        command.Set(new StoredProgram(canvas), "100 + 50 200 - 30");

        string[] splitResult = new[] { "100", "+", "50", "200", "-", "30" };

        // Should throw because parameter length != 2
        command.CheckParameters(splitResult);

        command.Compile();
    }
}
