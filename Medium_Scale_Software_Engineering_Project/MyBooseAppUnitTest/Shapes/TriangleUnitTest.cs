using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest.Shapes;

/// <summary>
/// Unit tests for the <see cref="AppTri"/> command using <see cref="TestAppCanvas"/> for thread-safe testing.
/// Tests cover valid dimensions, comma/space-separated input, expressions, zero values, and space-containing expressions.
/// </summary>
[TestClass]
public class TriangleUnitTest
{
    /// <summary>
    /// Verifies that <see cref="AppTri"/> executes successfully with valid literal width and height.
    /// </summary>
    [TestMethod]
    public void Triangle_ValidLiteralDimensions_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppTri(canvas);

        command.Set(new StoredProgram(canvas), "60 90");
        command.CheckParameters(new[] { "60", "90" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that <see cref="AppTri"/> executes successfully when width and height are provided with commas and spaces.
    /// </summary>
    [TestMethod]
    public void Triangle_ValidLiteralsWithCommaAndSpaces_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var command = new AppTri(canvas);

        command.Set(new StoredProgram(canvas), "  80 ,  120  ");
        command.CheckParameters(new[] { "80", "120" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that <see cref="AppTri"/> executes successfully with simple expressions (no spaces) for width and height.
    /// </summary>
    [TestMethod]
    public void Triangle_SimpleExpressionNoSpaces_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(300, 300);
        var program = new StoredProgram(canvas);
        var command = new AppTri(canvas);

        command.Set(program, "40+20 30*4");
        command.CheckParameters(new[] { "40+20", "30*4" });
        command.Compile();
        command.Execute();

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that <see cref="AppTri"/> throws a <see cref="BOOSEException"/> when width is zero.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(BOOSEException))]
    public void Triangle_ZeroDimension_ThrowsException()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppTri(canvas);
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
    /// Verifies that <see cref="AppTri"/> throws a <see cref="CommandException"/> when expressions contain spaces.
    /// Documents current limitation: inputs split into multiple parts fail validation.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void Triangle_ExpressionWithSpaces_ThrowsAsExpected()
    {
        var canvas = new TestAppCanvas(300, 300);
        try
        {
            var command = new AppTri(canvas);
            command.Set(new StoredProgram(canvas), "50 + 30 100 - 20");
            // After split → ["50", "+", "30", "100", "-", "20"] → length != 2 → throws
            command.CheckParameters(new[] { "50", "+", "30", "100", "-", "20" });
            command.Compile();
            command.Execute();
        }
        finally
        {
            canvas.Dispose();
        }
    }
}
