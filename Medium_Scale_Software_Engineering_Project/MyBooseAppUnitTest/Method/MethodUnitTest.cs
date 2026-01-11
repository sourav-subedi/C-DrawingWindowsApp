using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Methods;

/// <summary>
/// Simple unit tests for custom method calling system (Method + Call + EndMethod)
/// Tests are kept very minimal and literal
/// </summary>
[TestClass]
public class MethodCallUnitTest
{
    /// <summary>
    /// Test 1: Very basic void method declaration + call
    /// Checks that method can be declared and called without crashing
    /// </summary>
    [TestMethod]
    public void MethodCall_VoidMethod_NoArguments_ExecutesWithoutException()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new AppStoredProgram(canvas);

        // method drawSomething
        var methodCmd = new AppMethod();
        methodCmd.Set(program, "drawSomething");
        methodCmd.Compile();
        program.Add(methodCmd);

        // (imagine empty body here)

        var endMethodCmd = new AppEndMethod();
        endMethodCmd.Set(program, "");
        endMethodCmd.Compile();
        program.Add(endMethodCmd);

        // call drawSomething
        var callCmd = new AppCall();
        callCmd.Set(program, "drawSomething");
        callCmd.Compile();
        program.Add(callCmd);

        program.SetSyntaxStatus(true);
        program.ResetProgram();

        // Should not throw exception
        program.Run();

        Assert.IsTrue(true, "Void method declaration + call executed without exception");

        canvas.Dispose();
    }

    /// <summary>
    /// Test 2: Method with int return type + simple call
    /// Mainly checks that declaration + call doesn't crash
    /// </summary>
    [TestMethod]
    public void MethodCall_IntReturnMethod_NoArguments_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new AppStoredProgram(canvas);

        // method int getNumber
        var methodCmd = new AppMethod();
        methodCmd.Set(program, "int getNumber");
        methodCmd.Compile();
        program.Add(methodCmd);

        // (body would set getNumber = 42, but we skip for minimal test)

        var endMethodCmd = new AppEndMethod();
        endMethodCmd.Set(program, "");
        endMethodCmd.Compile();
        program.Add(endMethodCmd);

        // call getNumber
        var callCmd = new AppCall();
        callCmd.Set(program, "getNumber");
        callCmd.Compile();
        program.Add(callCmd);

        program.SetSyntaxStatus(true);
        program.ResetProgram();

        // Just testing that it runs without crashing
        program.Run();

        Assert.IsTrue(true, "Method with int return type + call executed without exception");

        canvas.Dispose();
    }

    /// <summary>
    /// Test 3: Method with two int parameters + call with arguments
    /// Checks basic parameter passing path (only existence, no value checking)
    /// </summary>
    [TestMethod]
    public void MethodCall_MethodWithTwoIntParameters_CanBeCalled()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new AppStoredProgram(canvas);

        // method int add int a, int b
        var methodCmd = new AppMethod();
        methodCmd.Set(program, "int add int a, int b");
        methodCmd.Compile();
        program.Add(methodCmd);

        // (body would compute a + b, skipped in minimal test)

        var endMethodCmd = new AppEndMethod();
        endMethodCmd.Set(program, "");
        endMethodCmd.Compile();
        program.Add(endMethodCmd);

        // call add 10 20
        var callCmd = new AppCall();
        callCmd.Set(program, "add 10 20");
        callCmd.Compile();
        program.Add(callCmd);

        program.SetSyntaxStatus(true);
        program.ResetProgram();

        // Should not crash during parameter creation & call
        program.Run();

        Assert.IsTrue(true, "Method with two int parameters + call with literals executed successfully");

        canvas.Dispose();
    }
}