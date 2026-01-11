using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest;

/// <summary>
/// Unit tests for <see cref="AppReal"/> (real/double variable declaration, assignment, and expressions)
/// using <see cref="StoredProgram"/> for variable management.
/// Tests include default initialization, literal and expression assignment, re-assignment, and error handling.
/// </summary>
[TestClass]
public class AppRealUnitTest
{
    /// <summary>
    /// Verifies that declaring a real variable without an initial value
    /// defaults the variable to 0.0.
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

    /// <summary>
    /// Verifies that declaring a real variable with a literal value
    /// initializes it correctly.
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

    /// <summary>
    /// Verifies that declaring a real variable with a simple expression
    /// evaluates and stores the correct result.
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

    /// <summary>
    /// Verifies that re-assigning an existing real variable with a new value or expression
    /// updates its value correctly.
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

    /// <summary>
    /// Verifies that declaring a real variable with an invalid expression
    /// throws a <see cref="CommandException"/>.
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

    /// <summary>
    /// Verifies that assigning an invalid expression to an existing real variable
    /// throws a <see cref="CommandException"/>.
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
