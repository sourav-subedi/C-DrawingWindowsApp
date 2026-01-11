using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Shapes;

/// <summary>
/// Unit tests for the MoveTo command using TestAppCanvas for thread-safe testing
/// Tests are adjusted for current Set() behavior (splits on comma AND space)
/// </summary>
[TestClass]
public class MoveToUnitTest
{
    // 1. Valid literal coordinates with space separator (current supported style)
    /// <summary>
    /// Tests MoveTo with valid literal integers separated by space - should execute successfully
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

    // 2. Valid literals with comma and spaces (should still work after trimming)
    /// <summary>
    /// Tests MoveTo with comma + spaces - current split removes empty entries
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

    // 3. Simple expression without spaces inside (supported by current split)
    /// <summary>
    /// Tests MoveTo with simple expressions that contain no spaces - should work
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

    // 4. Missing second parameter - should throw
    /// <summary>
    /// Tests MoveTo with only one parameter - expects CommandException
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

    // 5. Invalid non-numeric X - should throw during Execute
    /// <summary>
    /// Tests MoveTo with invalid X value - expects CommandException
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

    // 6. Expression with spaces inside should fail (matches current implementation)
    /// <summary>
    /// Tests that expressions containing spaces are NOT supported in current version
    /// (this test documents/guards the current limitation)
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void DrawTo_ExpressionWithSpaces_ThrowsAsExpected()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppDrawTo(canvas);

        // We expect this input to be split into too many pieces
        command.Set(new StoredProgram(canvas), "100 + 50 200 - 30");

        // Show what the internal array actually becomes after split
        string[] splitResult = new[] { "100", "+", "50", "200", "-", "30" };

        // This should throw because length != 2
        command.CheckParameters(splitResult);

        command.Compile();
    }
}