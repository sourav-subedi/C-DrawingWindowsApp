using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSE;
using MYBooseApp;

namespace MyBooseAppUnitTest
{
    /// <summary>
    /// Tests for AppInt (Facility 5 – Replaced Int).
    /// </summary>
    [TestClass]
    public class AppIntTests
    {
        /// <summary>
        /// Tests valid integer assignment.
        /// </summary>
        [TestMethod]
        public void AppInt_ValidInteger_AssignedCorrectly()
        {
            var canvas = new MockCanvas();
            var program = new AppStoredProgram(canvas);
            var parser = new AppParser(new CommandFactory(), program);

            parser.ParseProgram("int x = 10");
            program.Run();

            Assert.AreEqual(10, program.GetVariable("x").Value);
        }

        /// <summary>
        /// Tests rejection of fractional value.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(StoredProgramException))]
        public void AppInt_FractionalValue_ThrowsException()
        {
            var canvas = new MockCanvas();
            var program = new AppStoredProgram(canvas);
            var parser = new AppParser(new CommandFactory(), program);

            parser.ParseProgram("int x = 3.14");
            program.Run();
        }

        /// <summary>
        /// Tests rejection of invalid integer format.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(StoredProgramException))]
        public void AppInt_InvalidValue_ThrowsException()
        {
            var canvas = new MockCanvas();
            var program = new AppStoredProgram(canvas);
            var parser = new AppParser(new CommandFactory(), program);

            parser.ParseProgram("int x = abc");
            program.Run();
        }
    }
}
