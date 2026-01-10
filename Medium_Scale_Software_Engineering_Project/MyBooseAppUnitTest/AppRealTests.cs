using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MYBooseApp;
using MyBooseAppUnitTest;

[TestClass]
public class AppRealTests
{
    [TestMethod]
    public void AppReal_ValidDecimal_AssignedCorrectly()
    {
        // Arrange
        var canvas = new MockCanvas();
        var program = new AppStoredProgram(canvas); // Use your AppStoredProgram
        var factory = new AppCommandFactory();
        var parser = new AppParser(factory, program);

        // Create AppReal variable using BOOSE's standard workflow
        var realVar = new AppReal();
        realVar.Set(program, "x = 2.5"); // Sets Program, VarName, and Expression

        // Act
        realVar.Compile();
        realVar.Execute();

        // Assert
        Assert.AreEqual(2.5, realVar.Value, 0.001, "AppReal value was not assigned correctly.");
    }

    [TestMethod]
    [ExpectedException(typeof(StoredProgramException))]  // <-- Correct exception type
    public void AppReal_InvalidDecimal_ThrowsException()
    {
        var canvas = new MockCanvas();
        var program = new AppStoredProgram(canvas);
        var factory = new AppCommandFactory();
        var parser = new AppParser(factory, program);

        var realVar = new AppReal();
        realVar.Set(program, "x = invalid"); // Invalid decimal expression

        realVar.Compile();
        realVar.Execute();  // This will throw StoredProgramException
    }

}
