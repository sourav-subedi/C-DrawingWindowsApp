using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Shapes;

/// <summary>
/// Unit tests for the DrawTo command using TestAppCanvas for thread-safe testing
/// Tests are adjusted for current Set() behavior (splits on comma AND space)
/// </summary>
[TestClass]
public class DrawToUnitTest
{
    // 1. Valid literal coordinates with space separator (current supported style)
    /// <summary>
    /// Tests DrawTo with valid literal integers separated by space - should execute successfully
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

    // 2. Valid literals with comma and spaces (current split removes empty entries)
    /// <summary>
    /// Tests DrawTo with comma + spaces - should parse correctly
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

    // 3. Simple expression without spaces inside (supported by current split)
    /// <summary>
    /// Tests DrawTo with simple expressions that contain no spaces - should work
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

    // 4. Missing second parameter - should throw in CheckParameters
    /// <summary>
    /// Tests DrawTo with only one parameter - expects CommandException during validation
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

    // 5. Invalid non-numeric X value - should throw during Execute
    /// <summary>
    /// Tests DrawTo with invalid/non-numeric X coordinate - expects CommandException
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

    // 6. Expression with spaces inside - should fail (documents current limitation)
    /// <summary>
    /// Tests that expressions containing spaces are NOT supported in current version
    /// (this test guards/expects the limitation of the current Set() split logic)
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
            // This will split into more than 2 parts → CheckParameters should throw
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