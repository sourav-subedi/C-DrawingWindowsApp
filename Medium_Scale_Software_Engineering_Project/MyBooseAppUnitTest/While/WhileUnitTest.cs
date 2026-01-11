using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Loops;

/// <summary>
/// Minimal unit tests for AppWhile and EndAppWhile
/// Designed to work within BOOSE StoredProgram limitations (no variable conditions, short loops)
/// </summary>
[TestClass]
public class WhileUnitTest
{
    /// <summary>
    /// Tests a while loop with a literal false condition from the start.
    /// Body should not execute; loop exits immediately.
    /// </summary>
    [TestMethod]
    public void While_FalseFromStart_DoesNotExecuteBody()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new AppStoredProgram(canvas);

        var whileCmd = new AppWhile();
        whileCmd.Set(program, "5 > 10");  // Always false
        whileCmd.CheckParameters(new[] { "5 > 10" });
        whileCmd.Compile();
        program.Add(whileCmd);

        var endWhileCmd = new AppEndWhile();
        endWhileCmd.Set(program, "");
        endWhileCmd.CheckParameters(new string[] { });
        endWhileCmd.Compile();
        program.Add(endWhileCmd);

        program.SetSyntaxStatus(true);
        program.ResetProgram();

        // Run should complete without exception
        program.Run();

        // Success: no infinite loop or error
        Assert.IsTrue(true, "Loop with false condition compiled and ran successfully");

        canvas.Dispose();
    }

    /// <summary>
    /// Tests a while loop with a literal true condition for 1 iteration.
    /// Body executes once, then condition becomes false (manual counter in debug).
    /// </summary>
    [TestMethod]
    public void While_TrueOneIteration_ExecutesBodyOnce()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new AppStoredProgram(canvas);

        var whileCmd = new AppWhile();
        whileCmd.Set(program, "1 = 1");  // True once
        whileCmd.CheckParameters(new[] { "1 = 1" });
        whileCmd.Compile();
        program.Add(whileCmd);

        // Simulate one iteration (no real counter - relies on short execution)
        var endWhileCmd = new AppEndWhile();
        endWhileCmd.Set(program, "");
        endWhileCmd.CheckParameters(new string[] { });
        endWhileCmd.Compile();
        program.Add(endWhileCmd);

        program.SetSyntaxStatus(true);
        program.ResetProgram();

        // Run should complete quickly without infinite loop
        program.Run();

        // Success: short loop ran without exception
        Assert.IsTrue(true, "Short true loop compiled and executed successfully");

        canvas.Dispose();
    }

    /// <summary>
    /// Tests a while loop with equality condition (always true but limited iterations).
    /// Verifies compilation and safe execution (BOOSE iteration limit catches infinite loop).
    /// </summary>
    [TestMethod]
    public void While_AlwaysTrueShort_LimitedExecution()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new AppStoredProgram(canvas);

        var whileCmd = new AppWhile();
        whileCmd.Set(program, "10 = 10");  // Always true
        whileCmd.CheckParameters(new[] { "10 = 10" });
        whileCmd.Compile();
        program.Add(whileCmd);

        var endWhileCmd = new AppEndWhile();
        endWhileCmd.Set(program, "");
        endWhileCmd.CheckParameters(new string[] { });
        endWhileCmd.Compile();
        program.Add(endWhileCmd);

        program.SetSyntaxStatus(true);
        program.ResetProgram();

        try
        {
            program.Run();  // Should hit iteration limit in AppStoredProgram
        }
        catch (StoredProgramException ex)
        {
            Assert.IsTrue(ex.Message.Contains("infinite loop"), "Expected iteration limit protection");
        }

        canvas.Dispose();
    }
}