using MYBooseApp;
using BOOSE;

namespace UnitTests.IfElse;

/// <summary>
/// If command unit tests using TestAppCanvas for thread-safe testing
/// </summary>
[TestClass]
public class IfUnitTest
{
    /// <summary>
    /// Commands If with valid condition that evaluates to true, checks for successful execution
    /// </summary>
    [TestMethod]
    public void If_ConditionTrue_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new AppStoredProgram(canvas);

        AppIf AppIf = new AppIf();
        AppIf.Set(program, "5 > 3");
        AppIf.CheckParameters(new[] { "5 > 3" });
        AppIf.Compile();
        program.Add(AppIf);

        AppEndIf AppEndIf = new AppEndIf();
        AppEndIf.Set(program, "");
        AppEndIf.CheckParameters(new string[] { });
        AppEndIf.Compile();
        program.Add(AppEndIf);

        program.SetSyntaxStatus(true);
        program.Run();
        canvas.Dispose();
    }

    /// <summary>
    /// Commands If with valid condition that evaluates to false, checks for successful execution
    /// </summary>
    [TestMethod]
    public void If_ConditionFalse_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new AppStoredProgram(canvas);

        AppIf AppIf = new AppIf();
        AppIf.Set(program, "3 > 5");
        AppIf.CheckParameters(new[] { "3 > 5" });
        AppIf.Compile();
        program.Add(AppIf);

        AppEndIf AppEndIf = new AppEndIf();
        AppEndIf.Set(program, "");
        AppEndIf.CheckParameters(new string[] { });
        AppEndIf.Compile();
        program.Add(AppEndIf);

        program.SetSyntaxStatus(true);
        program.Run();
        canvas.Dispose();
    }

    /// <summary>
    /// Commands If with valid equality condition, checks for successful execution
    /// </summary>
    [TestMethod]
    public void If_EqualityCondition_ExecutesSuccessfully()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new AppStoredProgram(canvas);

        AppIf AppIf = new AppIf();
        AppIf.Set(program, "5 = 5");
        AppIf.CheckParameters(new[] { "5 = 5" });
        AppIf.Compile();
        program.Add(AppIf);

        AppEndIf AppEndIf = new AppEndIf();
        AppEndIf.Set(program, "");
        AppEndIf.CheckParameters(new string[] { });
        AppEndIf.Compile();
        program.Add(AppEndIf);

        program.SetSyntaxStatus(true);
        program.Run();
        canvas.Dispose();
    }
}