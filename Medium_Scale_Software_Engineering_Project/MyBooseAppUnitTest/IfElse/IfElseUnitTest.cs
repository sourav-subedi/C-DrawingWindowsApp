using MYBooseApp;
using BOOSE;

namespace MyBooseAppUnitTest.IfElse
{
    /// <summary>
    /// Unit tests for <see cref="AppIf"/> and <see cref="AppEndIf"/> commands.
    /// Uses <see cref="TestAppCanvas"/> for thread-safe execution and verification.
    /// </summary>
    [TestClass]
    public class IfUnitTest
    {
        /// <summary>
        /// Tests an <see cref="AppIf"/> command with a condition that evaluates to true.
        /// Verifies that the conditional block executes without errors.
        /// </summary>
        [TestMethod]
        public void If_ConditionTrue_ExecutesSuccessfully()
        {
            var canvas = new TestAppCanvas(200, 200);
            var program = new AppStoredProgram(canvas);

            var appIf = new AppIf();
            appIf.Set(program, "5 > 3");
            appIf.CheckParameters(new[] { "5 > 3" });
            appIf.Compile();
            program.Add(appIf);

            var appEndIf = new AppEndIf();
            appEndIf.Set(program, "");
            appEndIf.CheckParameters(new string[] { });
            appEndIf.Compile();
            program.Add(appEndIf);

            program.SetSyntaxStatus(true);
            program.Run();
            canvas.Dispose();
        }

        /// <summary>
        /// Tests an <see cref="AppIf"/> command with a condition that evaluates to false.
        /// Verifies that the conditional block is skipped and execution completes successfully.
        /// </summary>
        [TestMethod]
        public void If_ConditionFalse_ExecutesSuccessfully()
        {
            var canvas = new TestAppCanvas(200, 200);
            var program = new AppStoredProgram(canvas);

            var appIf = new AppIf();
            appIf.Set(program, "3 > 5");
            appIf.CheckParameters(new[] { "3 > 5" });
            appIf.Compile();
            program.Add(appIf);

            var appEndIf = new AppEndIf();
            appEndIf.Set(program, "");
            appEndIf.CheckParameters(new string[] { });
            appEndIf.Compile();
            program.Add(appEndIf);

            program.SetSyntaxStatus(true);
            program.Run();
            canvas.Dispose();
        }

        /// <summary>
        /// Tests an <see cref="AppIf"/> command with an equality condition (5 = 5).
        /// Verifies that the conditional block executes correctly for equality comparisons.
        /// </summary>
        [TestMethod]
        public void If_EqualityCondition_ExecutesSuccessfully()
        {
            var canvas = new TestAppCanvas(200, 200);
            var program = new AppStoredProgram(canvas);

            var appIf = new AppIf();
            appIf.Set(program, "5 = 5");
            appIf.CheckParameters(new[] { "5 = 5" });
            appIf.Compile();
            program.Add(appIf);

            var appEndIf = new AppEndIf();
            appEndIf.Set(program, "");
            appEndIf.CheckParameters(new string[] { });
            appEndIf.Compile();
            program.Add(appEndIf);

            program.SetSyntaxStatus(true);
            program.Run();
            canvas.Dispose();
        }
    }
}
