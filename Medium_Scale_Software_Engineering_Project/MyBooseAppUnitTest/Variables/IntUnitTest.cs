using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

/// <summary>
/// Unit tests for the AppInt (integer variable declaration and assignment)
/// using StoredProgram for variable management
/// </summary>
[TestClass]
public class AppIntUnitTest
{
    // 1. Declare integer without initialization → defaults to 0
    /// <summary>
    /// Tests simple declaration "int count" - should create variable with value 0
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

    // 2. Declare with literal initialization
    /// <summary>
    /// Tests declaration with literal value "int score = 42" - should set correct value
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

    // 3. Declare with simple expression
    /// <summary>
    /// Tests declaration with expression "int total = 10 + 35" - should evaluate and store
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

    // 4. Re-assign existing variable with new expression
    /// <summary>
    /// Tests re-assignment "x = 100" after declaration - should update value
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

    // 5. Invalid expression during declaration → should throw
    /// <summary>
    /// Tests declaration with invalid expression "int bad = hello" - expects CommandException
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

    // 6. Invalid expression during assignment → should throw
    /// <summary>
    /// Tests assignment with invalid expression "x = abc" - expects CommandException
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