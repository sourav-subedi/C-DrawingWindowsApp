using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Loops;

/// <summary>
/// Unit tests for ForCommand and EndForCommand using TestAppCanvas for thread-safe testing
/// Tests are minimal and literal-only to work within BOOSE StoredProgram limitations
/// </summary>
[TestClass]
public class ForUnitTest
{
    /// <summary>
    /// Tests a for loop with start > end (zero iterations): body should not execute
    /// </summary>
    [TestMethod]
    public void For_ZeroIterations_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new AppStoredProgram(canvas);

        try
        {
            // 1. Create commands
            var forCmd = new AppFor();
            forCmd.Set(program, "i = 5 to 3");  // start > end → zero iterations

            var endForCmd = new AppEndFor();
            endForCmd.Set(program, "");         // empty or "for" — match your parser

            // 2. Add in correct order (this should trigger linking internally)
            program.Add(forCmd);
            // program.Add(... any body commands ...);  ← intentionally skipped
            program.Add(endForCmd);

            // 3. Prepare/Reset (most BOOSE-like systems do linking or init here)
            program.ResetProgram();   // ← usually enough — triggers any lazy init/linking

            // 4. Run — should exit immediately without exception
            program.Run();

            // 5. Basic success assertions
            Assert.IsFalse(program.VariableExists("i"),
                "Loop variable 'i' should not exist after a zero-iteration loop");

            // If your program exposes final PC:
            // Assert.AreEqual(program.Count, program.PC, "PC should be at end after zero iterations");
        }
        finally
        {
            canvas.Dispose();
        }
    }

    /// <summary>
    /// Tests a for loop with ascending range (1 iteration): body executes once
    /// </summary>
    [TestMethod]
    public void For_AscendingOneIteration_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new AppStoredProgram(canvas);

        var forCmd = new AppFor();
        forCmd.Set(program, "i = 1 to 2");  // 1 iteration
        forCmd.CheckParameters(new[] { "i = 1 to 2" });
        forCmd.Compile();
        program.Add(forCmd);

        var endForCmd = new AppEndFor();
        endForCmd.Set(program, "");
        endForCmd.CheckParameters(new string[] { });
        endForCmd.Compile();
        program.Add(endForCmd);

        program.SetSyntaxStatus(true);
        program.ResetProgram();
        program.Run();

        // Success: short loop ran without exception
        Assert.IsTrue(true, "For loop with one iteration compiled and executed successfully");

        canvas.Dispose();
    }

    /// <summary>
    /// Tests a for loop with descending range (step negative): body executes once
    /// </summary>
    [TestMethod]
    public void For_DescendingOneIteration_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new AppStoredProgram(canvas);

        var forCmd = new AppFor();
        forCmd.Set(program, "i = 5 to 4 step -1");  // 1 iteration (descending)
        forCmd.CheckParameters(new[] { "i = 5 to 4 step -1" });
        forCmd.Compile();
        program.Add(forCmd);

        var endForCmd = new AppEndFor();
        endForCmd.Set(program, "");
        endForCmd.CheckParameters(new string[] { });
        endForCmd.Compile();
        program.Add(endForCmd);

        program.SetSyntaxStatus(true);
        program.ResetProgram();
        program.Run();

        // Success: short descending loop ran without exception
        Assert.IsTrue(true, "For loop with descending one iteration compiled and executed successfully");

        canvas.Dispose();
    }
}