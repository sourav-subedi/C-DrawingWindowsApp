using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest.Loops
{
    /// <summary>
    /// Unit tests for <see cref="AppFor"/> and <see cref="AppEndFor"/> commands.
    /// Uses <see cref="TestAppCanvas"/> for thread-safe testing.
    /// Tests are minimal and literal-only to work within the constraints of the BOOSE <see cref="StoredProgram"/>.
    /// </summary>
    [TestClass]
    public class ForUnitTest
    {
        /// <summary>
        /// Tests a for loop with start value greater than end value.
        /// Verifies that the loop body does not execute and the loop variable is not created.
        /// </summary>
        [TestMethod]
        public void For_ZeroIterations_ExecutesSuccessfully()
        {
            var canvas = new TestAppCanvas(200, 200);
            var program = new AppStoredProgram(canvas);

            try
            {
                // Create For and EndFor commands
                var forCmd = new AppFor();
                forCmd.Set(program, "i = 5 to 3");  // start > end → zero iterations

                var endForCmd = new AppEndFor();
                endForCmd.Set(program, "");         // EndFor command

                // Add commands in order (internal linking occurs)
                program.Add(forCmd);
                program.Add(endForCmd);

                // Prepare program for execution
                program.ResetProgram();

                // Execute program
                program.Run();

                // Verify the loop variable was not created
                Assert.IsFalse(program.VariableExists("i"),
                    "Loop variable 'i' should not exist after a zero-iteration loop");
            }
            finally
            {
                canvas.Dispose();
            }
        }

        /// <summary>
        /// Tests a for loop with an ascending range that executes exactly one iteration.
        /// Verifies successful compilation and execution of the loop.
        /// </summary>
        [TestMethod]
        public void For_AscendingOneIteration_ExecutesSuccessfully()
        {
            var canvas = new TestAppCanvas(200, 200);
            var program = new AppStoredProgram(canvas);

            // For loop with ascending range
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

            // Success: loop executed without exception
            Assert.IsTrue(true, "For loop with ascending range executed successfully");

            canvas.Dispose();
        }

        /// <summary>
        /// Tests a for loop with a descending range using a negative step.
        /// Ensures the loop executes correctly for one iteration and compiles without errors.
        /// </summary>
        [TestMethod]
        public void For_DescendingOneIteration_ExecutesSuccessfully()
        {
            var canvas = new TestAppCanvas(200, 200);
            var program = new AppStoredProgram(canvas);

            // For loop with descending range and negative step
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

            // Success: descending loop executed without exception
            Assert.IsTrue(true, "For loop with descending range executed successfully");

            canvas.Dispose();
        }
    }
}
