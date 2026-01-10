using BOOSE;
using MYBooseApp;
using MyBooseAppUnitTest;

/// <summary>
/// Tests for AppArray (Facility 5 – Replaced Array).
/// </summary>
[TestClass]
public class AppArrayTests
{
    [TestMethod]
    public void AppArray_ValidDeclaration_DoesNotThrow()
    {
        var canvas = new MockCanvas();
        var program = new StoredProgram(canvas);
        var factory = new AppCommandFactory();
        var parser = new AppParser(factory, program);

        string code = "array a 10 0";  // must match parser expected tokens
        parser.ParseProgram(code.Trim());  // should pass without exception
    }





    [TestMethod]
    [ExpectedException(typeof(ParserException))]
    public void AppArray_InvalidDeclaration_ThrowsParserException()
    {
        var canvas = new MockCanvas();
        var program = new StoredProgram(canvas);
        var factory = new AppCommandFactory();
        var parser = new AppParser(factory, program);

        string code = "array a"; // invalid

        parser.ParseProgram(code); // ParserException will be thrown
    }


}
