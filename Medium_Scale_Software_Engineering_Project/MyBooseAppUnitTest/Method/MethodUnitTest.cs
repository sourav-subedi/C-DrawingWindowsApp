using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest.Methods
{
    /// <summary>
    /// Unit tests for the custom method system (<see cref="AppMethod"/>, <see cref="AppCall"/>, <see cref="AppEndMethod"/>).
    /// Focused on verifying that methods can be declared, called, and executed correctly.
    /// Minimal logic; primarily tests declaration, invocation, and parameter handling.
    /// </summary>
    [TestClass]
    public class MethodCallUnitTest
    {
        /// <summary>
        /// Verifies that a void method with no parameters can be declared and called without exceptions.
        /// Ensures basic method declaration and execution functionality.
        /// </summary>
        [TestMethod]
        public void MethodCall_VoidMethod_NoArguments_ExecutesWithoutException()
        {
            var canvas = new TestAppCanvas(200, 200);
            var program = new AppStoredProgram(canvas);

            // Declare void method: drawSomething
            var methodCmd = new AppMethod();
            methodCmd.Set(program, "drawSomething");
            methodCmd.Compile();
            program.Add(methodCmd);

            // End of method
            var endMethodCmd = new AppEndMethod();
            endMethodCmd.Set(program, "");
            endMethodCmd.Compile();
            program.Add(endMethodCmd);

            // Call drawSomething
            var callCmd = new AppCall();
            callCmd.Set(program, "drawSomething");
            callCmd.Compile();
            program.Add(callCmd);

            program.SetSyntaxStatus(true);
            program.ResetProgram();

            // Execution should not throw
            program.Run();

            Assert.IsTrue(true, "Void method declaration + call executed without exception");

            canvas.Dispose();
        }

        /// <summary>
        /// Verifies that a method returning <see cref="int"/> with no parameters can be declared and invoked safely.
        /// Ensures that methods with return types execute without errors, even without parameters.
        /// </summary>
        [TestMethod]
        public void MethodCall_IntReturnMethod_NoArguments_ExecutesSuccessfully()
        {
            var canvas = new TestAppCanvas(200, 200);
            var program = new AppStoredProgram(canvas);

            // Declare method: int getNumber
            var methodCmd = new AppMethod();
            methodCmd.Set(program, "int getNumber");
            methodCmd.Compile();
            program.Add(methodCmd);

            // End of method
            var endMethodCmd = new AppEndMethod();
            endMethodCmd.Set(program, "");
            endMethodCmd.Compile();
            program.Add(endMethodCmd);

            // Call getNumber
            var callCmd = new AppCall();
            callCmd.Set(program, "getNumber");
            callCmd.Compile();
            program.Add(callCmd);

            program.SetSyntaxStatus(true);
            program.ResetProgram();

            // Execution should not throw
            program.Run();

            Assert.IsTrue(true, "Method with int return type + call executed without exception");

            canvas.Dispose();
        }

        /// <summary>
        /// Verifies that a method with two <see cref="int"/> parameters can be declared and called with literal arguments.
        /// Ensures proper handling of parameter passing and method invocation.
        /// </summary>
        //[TestMethod]
        //public void MethodCall_MethodWithTwoIntParameters_CanBeCalled()
        //{
        //    var canvas = new TestAppCanvas(200, 200);
        //    var program = new AppStoredProgram(canvas);

        //    // Declare method: int add int a, int b
        //    var methodCmd = new AppMethod();
        //    methodCmd.Set(program, "int add int a, int b");
        //    methodCmd.Compile();
        //    program.Add(methodCmd);

        //    // End of method
        //    var endMethodCmd = new AppEndMethod();
        //    endMethodCmd.Set(program, "");
        //    endMethodCmd.Compile();
        //    program.Add(endMethodCmd);

        //    // Call add 10 20
        //    var callCmd = new AppCall();
        //    callCmd.Set(program, "add 10 20");
        //    callCmd.Compile();
        //    program.Add(callCmd);

        //    program.SetSyntaxStatus(true);
        //    program.ResetProgram();

        //    // Execution should not throw
        //    program.Run();

        //    Assert.IsTrue(true, "Method with two int parameters + call with literals executed successfully");

        //    canvas.Dispose();
        //}
    }
}
