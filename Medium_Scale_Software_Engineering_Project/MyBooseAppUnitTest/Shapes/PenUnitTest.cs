using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Shapes;

/// <summary>
/// Unit tests for the Pen command (RGB color) using TestAppCanvas for thread-safe testing
/// Tests are adjusted for current Set() behavior (splits on comma AND space)
/// </summary>
[TestClass]
public class PenUnitTest
{
    // 1. Valid literal RGB values
    /// <summary>
    /// Tests Pen with valid literal RGB values - should execute successfully
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

    // 2. Valid literals with comma and spaces
    /// <summary>
    /// Tests Pen with comma + spaces around RGB values - should parse correctly
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

    // 3. Simple expressions without spaces inside (supported by current split)
    /// <summary>
    /// Tests Pen with simple expressions (no spaces) that evaluate to valid RGB values
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

    // 4. Invalid RGB count (too few parameters) - should throw in CheckParameters
    /// <summary>
    /// Tests Pen with only two parameters - expects CommandException
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

    // 5. Out-of-range RGB value - should throw during Execute
    /// <summary>
    /// Tests Pen with an RGB value > 255 - expects CommandException
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

    // 6. Expression with spaces inside - should fail (current limitation)
    /// <summary>
    /// Documents that expressions containing spaces are not supported in current version
    /// (splits into multiple parts → validation fails)
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