using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest.Shapes;

/// <summary>
/// Unit tests for the <see cref="AppPen"/> command (RGB color) using <see cref="TestAppCanvas"/> for thread-safe testing.
/// These tests verify handling of valid RGB values, expressions, and invalid input for the pen color command.
/// </summary>
[TestClass]
public class PenUnitTest
{
    /// <summary>
    /// Verifies that <see cref="AppPen"/> executes successfully with valid literal RGB values.
    /// </summary>
    [TestMethod]
    public void Pen_ValidLiteralRGB_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppPen(canvas);

        command.Set(new StoredProgram(canvas), "255 128 0");
        command.CheckParameters(new[] { "255", "128", "0" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that <see cref="AppPen"/> executes successfully when RGB values contain commas and extra spaces.
    /// </summary>
    [TestMethod]
    public void Pen_ValidLiteralsWithCommaAndSpaces_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppPen(canvas);

        command.Set(new StoredProgram(canvas), "  0 ,  255 ,  128  ");
        command.CheckParameters(new[] { "0", "255", "128" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that <see cref="AppPen"/> executes successfully with simple expressions (no spaces) that evaluate to valid RGB values.
    /// </summary>
    [TestMethod]
    public void Pen_SimpleExpressionNoSpaces_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var program = new StoredProgram(canvas);
        var command = new AppPen(canvas);

        command.Set(program, "100+55 200-72 50*5");
        command.CheckParameters(new[] { "100+55", "200-72", "50*5" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that <see cref="AppPen"/> throws a <see cref="CommandException"/> when fewer than three RGB parameters are provided.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void Pen_TooFewParameters_ThrowsException()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppPen(canvas);
            command.Set(new StoredProgram(canvas), "255 128");
            command.CheckParameters(new[] { "255", "128" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }

    /// <summary>
    /// Verifies that <see cref="AppPen"/> throws a <see cref="CommandException"/> when an RGB value is out of range (>255).
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void Pen_OutOfRangeValue_ThrowsException()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppPen(canvas);
            command.Set(new StoredProgram(canvas), "300 100 50");
            command.CheckParameters(new[] { "300", "100", "50" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }

    /// <summary>
    /// Verifies that <see cref="AppPen"/> throws a <see cref="CommandException"/> when expressions contain spaces.
    /// This test documents a current limitation: inputs split into more than three parts fail validation.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void Pen_ExpressionWithSpaces_ThrowsAsExpected()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppPen(canvas);
            command.Set(new StoredProgram(canvas), "100 + 55 200 - 72 0");
            // After split → ["100", "+", "55", "200", "-", "72", "0"] → length != 3 → throws
            command.CheckParameters(new[] { "100", "+", "55", "200", "-", "72", "0" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }
}
