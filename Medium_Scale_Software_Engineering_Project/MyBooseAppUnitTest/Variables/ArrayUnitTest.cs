using MYBooseApp;
using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

/// <summary>
/// Unit tests for the array system: AppArray (declaration), AppPoke (write), PeekCommand (read)
/// </summary>
[TestClass]
public class ArrayUnitTest
{
    // 1. Declare int array successfully
    /// <summary>
    /// Tests declaration of an integer array "array int scores 5" - should create array with size 5
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

    // 2. Declare real array successfully
    /// <summary>
    /// Tests declaration of a real array "array real temps 10" - should create array with size 10
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

    // 3. Poke integer value into int array (literal)
    /// <summary>
    /// Tests poke command "poke scores 2 = 85" - should set index 2 to 85
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

    // 4. Poke real value into real array (expression)
    /// <summary>
    /// Tests poke with expression "poke temps 3 = 36.6 + 0.4" - should set index 3 to 37.0
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

    // 5. Peek from int array into int variable
    /// <summary>
    /// Tests peek "peek result = scores 2" after poke - should copy value 85 to result
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

    // 6. Index out of bounds on poke - should throw
    /// <summary>
    /// Tests poke with invalid index "poke scores 10 = 99" - expects CommandException
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
            pokeCmd.Execute();  // Should throw here
        }
        finally
        {
            canvas.Dispose();
        }
    }
}