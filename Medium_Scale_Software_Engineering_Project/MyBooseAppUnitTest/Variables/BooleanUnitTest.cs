using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest;

/// <summary>
/// Unit tests for <see cref="AppBoolean"/> (boolean variable declaration, assignment, and expressions)
/// using <see cref="StoredProgram"/> for variable management.
/// Tests include default initialization, literal assignment, invalid expression handling, and boolean operations.
/// </summary>
[TestClass]
public class AppBooleanUnitTest
{
    /// <summary>
    /// Verifies that declaring a boolean variable without an initial value
    /// defaults the variable to false.
    /// </summary>
    [TestMethod]
    public void AppBoolean_DeclareWithoutValue_DefaultsToFalse()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppBoolean();

        command.Set(program, "flag");
        command.Compile();

        var varObj = program.GetVariable("flag") as AppBoolean;
        Assert.IsNotNull(varObj);
        Assert.IsFalse(varObj.BoolValue);

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that declaring a boolean variable with a literal true
    /// sets the variable value to true.
    /// </summary>
    [TestMethod]
    public void AppBoolean_DeclareWithTrueLiteral_SetsTrue()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppBoolean();

        command.Set(program, "isActive = true");
        command.Compile();

        var varObj = program.GetVariable("isActive") as AppBoolean;
        Assert.IsNotNull(varObj);
        Assert.IsTrue(varObj.BoolValue);

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that declaring a boolean variable with a literal false
    /// sets the variable value to false.
    /// </summary>
    [TestMethod]
    public void AppBoolean_DeclareWithFalseLiteral_SetsFalse()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppBoolean();

        command.Set(program, "debug = false");
        command.Compile();

        var varObj = program.GetVariable("debug") as AppBoolean;
        Assert.IsNotNull(varObj);
        Assert.IsFalse(varObj.BoolValue);

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that assigning an invalid expression to a boolean variable
    /// throws a <see cref="CommandException"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void AppBoolean_InvalidExpression_ThrowsException()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppBoolean();

        try
        {
            // Declare first
            command.Set(program, "flag");
            command.Compile();

            // Invalid assignment
            command.Set(program, "flag = hello && world");
            command.Execute();  // Should throw here
        }
        finally
        {
            canvas.Dispose();
        }
    }

    /// <summary>
    /// Verifies that a boolean variable can be reassigned using other boolean variables
    /// in a logical expression and evaluates correctly.
    /// </summary>
    [TestMethod]
    public void AppBoolean_ReassignWithOtherBoolean_EvaluatesCorrectly()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);

        // Declare two booleans
        var cmd1 = new AppBoolean();
        cmd1.Set(program, "isActive = true");
        cmd1.Compile();

        var cmd2 = new AppBoolean();
        cmd2.Set(program, "debug = false");
        cmd2.Compile();

        // Declare status
        var cmd3 = new AppBoolean();
        cmd3.Set(program, "status");
        cmd3.Compile();

        // Assign expression using other booleans
        cmd3.Set(program, "status = isActive || debug");
        cmd3.Execute();

        var statusVar = program.GetVariable("status") as AppBoolean;
        Assert.IsNotNull(statusVar);
        Assert.IsTrue(statusVar.BoolValue);  // true || false = true

        canvas.Dispose();
    }
}
