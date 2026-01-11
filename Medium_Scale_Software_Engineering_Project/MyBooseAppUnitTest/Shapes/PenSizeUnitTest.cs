using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest.Shapes;

/// <summary>
/// Unit tests for the <see cref="AppPenSize"/> command using <see cref="TestAppCanvas"/> for thread-safe testing.
/// These tests verify handling of positive integers, expressions, and invalid input values for pen size.
/// </summary>
[TestClass]
public class PenSizeUnitTest
{
    /// <summary>
    /// Verifies that <see cref="AppPenSize"/> executes successfully with a valid positive literal integer.
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

    /// <summary>
    /// Verifies that <see cref="AppPenSize"/> executes correctly when the parameter contains extra spaces.
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

    /// <summary>
    /// Verifies that <see cref="AppPenSize"/> executes successfully with simple expressions containing no spaces.
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

    /// <summary>
    /// Verifies that <see cref="AppPenSize"/> throws a <see cref="CommandException"/> when the size is zero.
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

    /// <summary>
    /// Verifies that <see cref="AppPenSize"/> throws a <see cref="CommandException"/> when the size is negative.
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

    /// <summary>
    /// Verifies that <see cref="AppPenSize"/> throws a <see cref="CommandException"/> when the parameter is invalid/non-numeric.
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
