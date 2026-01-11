using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

/// <summary>
/// Unit tests for the AppReal (double/real variable declaration and assignment)
/// using StoredProgram for variable management
/// </summary>
[TestClass]
public class AppRealUnitTest
{
    // 1. Declare real without initialization → defaults to 0.0
    /// <summary>
    /// Tests simple declaration "real pi" - should create variable with value 0.0
    /// </summary>
    [TestMethod]
    public void AppReal_DeclareWithoutValue_DefaultsToZero()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppReal();

        command.Set(program, "pi");
        command.Compile();

        var varObj = program.GetVariable("pi") as AppReal;
        Assert.IsNotNull(varObj);
        Assert.AreEqual(0.0, varObj.RealValue, 0.0001);

        canvas.Dispose();
    }

    // 2. Declare with literal initialization
    /// <summary>
    /// Tests declaration with literal value "real temperature = 23.5" - should set correct value
    /// </summary>
    [TestMethod]
    public void AppReal_DeclareWithLiteral_InitializesCorrectly()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppReal();

        command.Set(program, "temperature = 23.5");
        command.Compile();

        var varObj = program.GetVariable("temperature") as AppReal;
        Assert.IsNotNull(varObj);
        Assert.AreEqual(23.5, varObj.RealValue, 0.0001);

        canvas.Dispose();
    }

    // 3. Declare with simple expression
    /// <summary>
    /// Tests declaration with expression "real area = 3.14 * 5 * 5" - should evaluate correctly
    /// </summary>
    [TestMethod]
    public void AppReal_DeclareWithExpression_EvaluatesCorrectly()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppReal();

        command.Set(program, "area = 3.14 * 5 * 5");
        command.Compile();

        var varObj = program.GetVariable("area") as AppReal;
        Assert.IsNotNull(varObj);
        Assert.AreEqual(78.5, varObj.RealValue, 0.0001);

        canvas.Dispose();
    }

    // 4. Re-assign existing variable with new value/expression
    /// <summary>
    /// Tests re-assignment "distance = 100.75" after declaration - should update value
    /// </summary>
    [TestMethod]
    public void AppReal_ReassignExistingVariable_UpdatesValue()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppReal();

        // First declare
        command.Set(program, "distance");
        command.Compile();

        var varObjInitial = program.GetVariable("distance") as AppReal;
        Assert.IsNotNull(varObjInitial);
        Assert.AreEqual(0.0, varObjInitial.RealValue, 0.0001);

        // Then assign new value
        command.Set(program, "distance = 100.75");
        command.Execute();

        var varObjAfter = program.GetVariable("distance") as AppReal;
        Assert.IsNotNull(varObjAfter);
        Assert.AreEqual(100.75, varObjAfter.RealValue, 0.0001);

        canvas.Dispose();
    }

    // 5. Invalid expression during declaration → should throw CommandException
    /// <summary>
    /// Tests declaration with invalid expression "real invalid = hello * 3.5" - expects CommandException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void AppReal_InvalidExpressionInDeclaration_ThrowsException()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppReal();

        try
        {
            command.Set(program, "invalid = hello * 3.5");
            command.Compile();  // Evaluation happens here → should throw
        }
        finally
        {
            canvas.Dispose();
        }
    }

    // 6. Invalid expression during assignment → should throw CommandException
    /// <summary>
    /// Tests assignment with invalid expression "distance = abc + 2.5" - expects CommandException
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void AppReal_InvalidExpressionInAssignment_ThrowsException()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppReal();

        try
        {
            // First declare normally
            command.Set(program, "distance");
            command.Compile();

            // Then invalid assignment
            command.Set(program, "distance = abc + 2.5");
            command.Execute();  // Evaluation happens here → should throw
        }
        finally
        {
            canvas.Dispose();
        }
    }
}