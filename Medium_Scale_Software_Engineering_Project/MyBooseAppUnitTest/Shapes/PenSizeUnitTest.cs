using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Shapes;

/// <summary>
/// Unit tests for the PenSize command using TestAppCanvas for thread-safe testing
/// Tests are adjusted for current Set() behavior (splits on comma AND space)
/// </summary>
[TestClass]
public class PenSizeUnitTest
{
    // 1. Valid literal positive size
    /// <summary>
    /// Tests PenSize with a valid positive literal integer - should execute successfully
    /// </summary>
    [TestMethod]
    public void PenSize_ValidLiteralPositive_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppPenSize(canvas);
        
        command.Set(new StoredProgram(canvas), "5");
        command.CheckParameters(new[] { "5" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    // 2. Valid literal with spaces around
    /// <summary>
    /// Tests PenSize with extra spaces around the parameter - should parse correctly
    /// </summary>
    [TestMethod]
    public void PenSize_ValidLiteralWithSpaces_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppPenSize(canvas);
        
        command.Set(new StoredProgram(canvas), "  8  ");
        command.CheckParameters(new[] { "8" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    // 3. Simple expression without spaces (supported by current split)
    /// <summary>
    /// Tests PenSize with a simple expression containing no spaces - should work
    /// </summary>
    [TestMethod]
    public void PenSize_SimpleExpressionNoSpaces_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var program = new StoredProgram(canvas);
        var command = new AppPenSize(canvas);
        
        command.Set(program, "3+2");
        command.CheckParameters(new[] { "3+2" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    // 4. Zero size - should throw CommandException
    /// <summary>
    /// Tests PenSize with zero value - expects CommandException (must be positive)
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void PenSize_ZeroSize_ThrowsException()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppPenSize(canvas);
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

    // 5. Negative size - should throw CommandException
    /// <summary>
    /// Tests PenSize with negative value - expects CommandException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void PenSize_NegativeSize_ThrowsException()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppPenSize(canvas);
            command.Set(new StoredProgram(canvas), "-4");
            command.CheckParameters(new[] { "-4" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }

    // 6. Invalid/non-numeric input - should throw during Execute
    /// <summary>
    /// Tests PenSize with invalid/non-numeric value - expects CommandException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void PenSize_InvalidValue_ThrowsException()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppPenSize(canvas);
            command.Set(new StoredProgram(canvas), "thick");
            command.CheckParameters(new[] { "thick" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }
}