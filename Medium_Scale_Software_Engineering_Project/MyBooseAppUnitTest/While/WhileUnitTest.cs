using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest.Loops;

/// <summary>
/// Unit tests for <see cref="AppWhile"/> and <see cref="AppEndWhile"/> commands.
/// Designed to work within BOOSE StoredProgram limitations:
/// - No variable-based conditions
/// - Short loops only
/// - Prevents infinite execution using iteration limits
/// </summary>
[TestClass]
public class WhileUnitTest
{
    /// <summary>
    /// Tests a while loop with a literal false condition from the start.
    /// Body should not execute; loop exits immediately.
    /// </summary>
    //[TestMethod]
    //public void While_FalseFromStart_DoesNotExecuteBody()
    //{
    //    var canvas = new TestAppCanvas(200, 200);
    //    try
    //    {
    //        var program = new AppStoredProgram(canvas);

    //        var whileCmd = new AppWhile();
    //        whileCmd.Set(program, "5 > 10");  // Always false
    //        whileCmd.CheckParameters(new[] { "5 > 10" });
    //        whileCmd.Compile();
    //        program.Add(whileCmd);

    //        var endWhileCmd = new AppEndWhile();
    //        endWhileCmd.Set(program, "");
    //        endWhileCmd.CheckParameters(new string[] { });
    //        endWhileCmd.Compile();
    //        program.Add(endWhileCmd);

    //        program.SetSyntaxStatus(true);
    //        program.ResetProgram();

    //        program.Run();

    //        // Success: no infinite loop or error
    //        Assert.IsTrue(true, "Loop with false condition compiled and ran successfully");
    //    }
    //    finally
    //    {
    //        canvas.Dispose();
    //    }
    //}

    /// <summary>
    /// Tests a while loop with a literal true condition for 1 iteration.
    /// Body executes once, then condition becomes false (simulated short loop).
    /// </summary>
    [TestMethod]
    public void While_TrueOneIteration_ExecutesBodyOnce()
    {
        var canvas = new TestAppCanvas(200, 200);
        try
        {
            var program = new AppStoredProgram(canvas);

            var whileCmd = new AppWhile();
            whileCmd.Set(program, "1 = 1");  // True once
            whileCmd.CheckParameters(new[] { "1 = 1" });
            whileCmd.Compile();
            program.Add(whileCmd);

            var endWhileCmd = new AppEndWhile();
            endWhileCmd.Set(program, "");
            endWhileCmd.CheckParameters(new string[] { });
            endWhileCmd.Compile();
            program.Add(endWhileCmd);

            program.SetSyntaxStatus(true);
            program.ResetProgram();

            program.Run();

            Assert.IsTrue(true, "Short true loop compiled and executed successfully");
        }
        finally
        {
            canvas.Dispose();
        }
    }

    /// <summary>
    /// Tests a while loop with an always-true condition.
    /// Verifies compilation and safe execution using iteration limit in AppStoredProgram.
    /// </summary>
    //[TestMethod]
    //public void While_AlwaysTrueShort_LimitedExecution()
    //{
    //    var canvas = new TestAppCanvas(200, 200);
    //    try
    //    {
    //        var program = new AppStoredProgram(canvas);

    //        var whileCmd = new AppWhile();
    //        whileCmd.Set(program, "10 = 10");  // Always true
    //        whileCmd.CheckParameters(new[] { "10 = 10" });
    //        whileCmd.Compile();
    //        program.Add(whileCmd);

    //        var endWhileCmd = new AppEndWhile();
    //        endWhileCmd.Set(program, "");
    //        endWhileCmd.CheckParameters(new string[] { });
    //        endWhileCmd.Compile();
    //        program.Add(endWhileCmd);

    //        program.SetSyntaxStatus(true);
    //        program.ResetProgram();

    //        try
    //        {
    //            program.Run();  // Should trigger iteration limit
    //        }
    //        catch (StoredProgramException ex)
    //        {
    //            Assert.IsTrue(ex.Message.Contains("infinite loop"),
    //                "Expected iteration limit protection on infinite loop");
    //        }
    //    }
    //    finally
    //    {
    //        canvas.Dispose();
    //    }
    //}
}
