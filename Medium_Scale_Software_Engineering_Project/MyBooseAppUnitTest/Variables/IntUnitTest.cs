using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest;

/// <summary>
/// Unit tests for <see cref="AppInt"/> (integer variable declaration, assignment, and expressions)
/// using <see cref="StoredProgram"/> for variable management.
/// Tests include default initialization, literal and expression assignment, re-assignment, and error handling.
/// </summary>
[TestClass]
public class AppIntUnitTest
{
    /// <summary>
    /// Verifies that declaring an integer variable without an initial value
    /// defaults the variable to 0.
    /// </summary>
    [TestMethod]
    public void AppInt_DeclareWithoutValue_DefaultsToZero()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppInt();

        command.Set(program, "count");
        command.Compile();

        Assert.IsTrue(program.VariableExists("count"));
        Assert.AreEqual("0", program.GetVarValue("count"));

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that declaring an integer variable with a literal value
    /// initializes it correctly.
    /// </summary>
    [TestMethod]
    public void AppInt_DeclareWithLiteral_InitializesCorrectly()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppInt();

        command.Set(program, "score = 42");
        command.Compile();

        Assert.IsTrue(program.VariableExists("score"));
        Assert.AreEqual("42", program.GetVarValue("score"));

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that declaring an integer variable with a simple expression
    /// evaluates and stores the correct result.
    /// </summary>
    [TestMethod]
    public void AppInt_DeclareWithExpression_EvaluatesCorrectly()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppInt();

        command.Set(program, "total = 10 + 35");
        command.Compile();

        Assert.IsTrue(program.VariableExists("total"));
        Assert.AreEqual("45", program.GetVarValue("total"));

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that re-assigning an existing integer variable with a new expression
    /// updates its value correctly.
    /// </summary>
    [TestMethod]
    public void AppInt_ReassignExistingVariable_UpdatesValue()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppInt();

        // First declare
        command.Set(program, "x");
        command.Compile();
        Assert.AreEqual("0", program.GetVarValue("x"));

        // Then assign new value
        command.Set(program, "x = 75 + 25");
        command.Execute();  // Re-assignment uses Execute()

        Assert.AreEqual("100", program.GetVarValue("x"));

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that declaring an integer variable with an invalid expression
    /// throws a <see cref="CommandException"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void AppInt_InvalidExpressionInDeclaration_ThrowsException()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppInt();

        try
        {
            command.Set(program, "bad = hello * 2");
            command.Compile();  // Evaluation happens here → should throw
        }
        finally
        {
            canvas.Dispose();
        }
    }

    /// <summary>
    /// Verifies that assigning an invalid expression to an existing integer variable
    /// throws a <see cref="CommandException"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void AppInt_InvalidExpressionInAssignment_ThrowsException()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppInt();

        try
        {
            // First declare normally
            command.Set(program, "x");
            command.Compile();

            // Then invalid assignment
            command.Set(program, "x = abc + 5");
            command.Execute();  // Evaluation happens here → should throw
        }
        finally
        {
            canvas.Dispose();
        }
    }
}
