using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest;

/// <summary>
/// Unit tests for the array system: <see cref="AppArray"/> (declaration),
/// <see cref="AppPoke"/> (write), and <see cref="AppPeek"/> (read).
/// Tests include declaration, poke, peek, expression evaluation, and index validation.
/// </summary>
[TestClass]
public class ArrayUnitTest
{
    /// <summary>
    /// Verifies that an integer array can be declared successfully with a given size.
    /// </summary>
    [TestMethod]
    public void Array_DeclareIntArray_Success()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppArray();

        command.Set(program, "int scores 5");
        command.Compile();

        Assert.IsTrue(program.VariableExists("scores"));
        var arr = program.GetVariable("scores") as AppArray;
        Assert.IsNotNull(arr);
        Assert.AreEqual(5, arr.Size);
        Assert.IsTrue(arr.IsIntArray());

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that a real array can be declared successfully with a given size.
    /// </summary>
    [TestMethod]
    public void Array_DeclareRealArray_Success()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var command = new AppArray();

        command.Set(program, "real temps 10");
        command.Compile();

        Assert.IsTrue(program.VariableExists("temps"));
        var arr = program.GetVariable("temps") as AppArray;
        Assert.IsNotNull(arr);
        Assert.AreEqual(10, arr.Size);
        Assert.IsTrue(arr.IsRealArray());

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that an integer value can be poked into an integer array at a specified index.
    /// </summary>
    [TestMethod]
    public void Poke_IntLiteral_SetsCorrectly()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var arrCmd = new AppArray();
        arrCmd.Set(program, "int scores 5");
        arrCmd.Compile();

        var pokeCmd = new AppPoke();
        pokeCmd.Set(program, "scores 2 = 85");
        pokeCmd.Compile();
        pokeCmd.Execute();

        var arr = program.GetVariable("scores") as AppArray;
        Assert.AreEqual(85, arr.GetIntArray(2));

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that a real expression can be poked into a real array at a specified index.
    /// </summary>
    [TestMethod]
    public void Poke_RealExpression_SetsCorrectly()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);
        var arrCmd = new AppArray();
        arrCmd.Set(program, "real temps 6");
        arrCmd.Compile();

        var pokeCmd = new AppPoke();
        pokeCmd.Set(program, "temps 3 = 36.6 + 0.4");
        pokeCmd.Compile();
        pokeCmd.Execute();

        var arr = program.GetVariable("temps") as AppArray;
        Assert.AreEqual(37.0, arr.GetRealArray(3), 0.0001);

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that a value can be peeked from an integer array into an integer variable.
    /// </summary>
    [TestMethod]
    public void Peek_IntArrayToIntVariable_CopiesCorrectly()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);

        // Declare array
        var arrCmd = new AppArray();
        arrCmd.Set(program, "int scores 5");
        arrCmd.Compile();

        // Poke value
        var pokeCmd = new AppPoke();
        pokeCmd.Set(program, "scores 2 = 85");
        pokeCmd.Compile();
        pokeCmd.Execute();

        // Declare target variable
        var intCmd = new AppInt();
        intCmd.Set(program, "result");
        intCmd.Compile();

        // Peek
        var peekCmd = new AppPeek();
        peekCmd.Set(program, "result = scores 2");
        peekCmd.Compile();
        peekCmd.Execute();

        var resultVar = program.GetVariable("result") as AppInt;
        Assert.AreEqual(85, resultVar.Value);

        canvas.Dispose();
    }

    /// <summary>
    /// Verifies that poking a value into an array at an out-of-bounds index throws a <see cref="CommandException"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CommandException))]
    public void Poke_IndexOutOfBounds_ThrowsException()
    {
        var canvas = new TestAppCanvas(200, 200);
        var program = new StoredProgram(canvas);

        var arrCmd = new AppArray();
        arrCmd.Set(program, "int scores 5");
        arrCmd.Compile();

        try
        {
            var pokeCmd = new AppPoke();
            pokeCmd.Set(program, "scores 10 = 99");
            pokeCmd.Compile();
            pokeCmd.Execute();  // Should throw
        }
        finally
        {
            canvas.Dispose();
        }
    }
}
