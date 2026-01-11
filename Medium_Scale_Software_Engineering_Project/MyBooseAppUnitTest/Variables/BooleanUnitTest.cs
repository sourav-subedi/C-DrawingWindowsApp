using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

/// <summary>
/// Unit tests for the AppBoolean (boolean variable declaration and assignment)
/// using StoredProgram for variable management
/// </summary>
[TestClass]
public class AppBooleanUnitTest
{
    // 1. Declare boolean without initialization → defaults to false
    /// <summary>
    /// Tests simple declaration "boolean flag" - should create variable with value false
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

    // 2. Declare with literal true
    /// <summary>
    /// Tests declaration with literal value "boolean isActive = true" - should set value to true
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

    // 3. Declare with literal false
    /// <summary>
    /// Tests declaration with literal value "boolean debug = false" - should set value to false
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


    // 4. Invalid expression during assignment → should throw CommandException
    /// <summary>
    /// Tests assignment with invalid expression "flag = hello and world" expects CommandException
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

    // 5. Re-assignment using existing boolean variable in expression
    /// <summary>
    /// Tests re-assignment using another boolean "status = isActive || debug" - evaluates correctly
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