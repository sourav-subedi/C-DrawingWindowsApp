using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest.Shapes;

/// <summary>
/// Unit tests for the <see cref="AppRect"/> command using <see cref="TestAppCanvas"/> for thread-safe testing.
/// These tests verify handling of valid rectangle dimensions, filled flags, expressions, and invalid input.
/// </summary>
[TestClass]
public class RectangleUnitTest
{
    /// <summary>
    /// Verifies that <see cref="AppRect"/> executes successfully with valid literal width and height.
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

    /// <summary>
    /// Verifies that <see cref="AppRect"/> executes successfully when width, height, and filled flag are provided with commas and spaces.
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

    /// <summary>
    /// Verifies that <see cref="AppRect"/> executes successfully with simple expressions (no spaces) for width and height.
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

    /// <summary>
    /// Verifies that <see cref="AppRect"/> throws a <see cref="CommandException"/> when width is zero.
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

    /// <summary>
    /// Verifies that <see cref="AppRect"/> throws a <see cref="CommandException"/> when width is invalid/non-numeric.
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

    /// <summary>
    /// Verifies that <see cref="AppRect"/> throws a <see cref="CommandException"/> when expressions contain spaces.
    /// Documents current limitation: inputs split into multiple parts fail validation.
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
