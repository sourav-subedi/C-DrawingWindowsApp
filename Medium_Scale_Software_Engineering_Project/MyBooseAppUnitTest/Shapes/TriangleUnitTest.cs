using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Shapes;

/// <summary>
/// Unit tests for the Triangle command using TestAppCanvas for thread-safe testing
/// Tests are adjusted for current Set() behavior (splits on comma AND space)
/// </summary>
[TestClass]
public class TriangleUnitTest
{
    // 1. Valid literal dimensions (space separated)
    /// <summary>
    /// Tests Triangle with valid literal width/height - should execute successfully
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

    // 2. Valid literals with comma and spaces
    /// <summary>
    /// Tests Triangle with comma + spaces around parameters - should parse correctly
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

    // 3. Simple expression without spaces (supported by current split)
    /// <summary>
    /// Tests Triangle with simple expressions (no spaces) for width/height - should work
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

    // 4. Zero dimension - should throw BOOSEException
    /// <summary>
    /// Tests Triangle with zero width - expects BOOSEException (must be positive)
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


    // 5. Expression with spaces inside - should fail (current limitation)
    /// <summary>
    /// Documents that expressions containing spaces are not supported in current version
    /// (splits into multiple parts → validation fails in CheckParameters)
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