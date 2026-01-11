using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Shapes;

/// <summary>
/// Unit tests for the Rectangle command using TestAppCanvas for thread-safe testing
/// Tests are adjusted for current Set() behavior (splits on comma AND space)
/// </summary>
[TestClass]
public class RectangleUnitTest
{
    // 1. Valid literal dimensions, no filled flag
    /// <summary>
    /// Tests Rectangle with valid literal width/height, no filled flag - should execute successfully
    /// </summary>
    [TestMethod]
    public void Rectangle_ValidLiteralDimensions_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppRect(canvas);

        command.Set(new StoredProgram(canvas), "120 80");
        command.CheckParameters(new[] { "120", "80" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    // 2. Valid literals with comma and spaces, filled = true
    /// <summary>
    /// Tests Rectangle with comma + spaces and filled flag - should parse and execute correctly
    /// </summary>
    [TestMethod]
    public void Rectangle_WithFilledTrue_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppRect(canvas);

        command.Set(new StoredProgram(canvas), "  150 ,  100 ,  true  ");
        command.CheckParameters(new[] { "150", "100", "true" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    // 3. Simple expressions without spaces (supported by current split)
    /// <summary>
    /// Tests Rectangle with simple expressions (no spaces) for width/height - should work
    /// </summary>
    [TestMethod]
    public void Rectangle_SimpleExpressionNoSpaces_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var program = new StoredProgram(canvas);
        var command = new AppRect(canvas);

        command.Set(program, "50+50 30*3");
        command.CheckParameters(new[] { "50+50", "30*3" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    // 4. Zero dimension - should throw CommandException
    /// <summary>
    /// Tests Rectangle with zero width - expects CommandException (must be positive)
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void Rectangle_ZeroDimension_ThrowsException()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppRect(canvas);
            command.Set(new StoredProgram(canvas), "0 100");
            command.CheckParameters(new[] { "0", "100" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }

    // 5. Invalid/non-numeric width - should throw during Execute
    /// <summary>
    /// Tests Rectangle with invalid/non-numeric width value - expects CommandException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void Rectangle_InvalidWidthValue_ThrowsException()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppRect(canvas);
            command.Set(new StoredProgram(canvas), "wide 80");
            command.CheckParameters(new[] { "wide", "80" });
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
    public void Rectangle_ExpressionWithSpaces_ThrowsAsExpected()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppRect(canvas);
            command.Set(new StoredProgram(canvas), "100 + 50 80 true");
            // After split → ["100", "+", "50", "80", "true"] → length != 2 or 3 → throws
            command.CheckParameters(new[] { "100", "+", "50", "80", "true" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }
}