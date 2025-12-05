using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSE;
using MYBooseApp;

namespace MyBooseAppUnitTest
{
    /// <summary>
    /// test class to test the moveto class
    /// </summary>
    [TestClass]
    public class MoveToTests
    {
        private AppCanvas canvas;
        private StoredProgram program;
        private AppCommandFactory factory;
        private AppParser parser;

        /// <summary>
        /// initialize the values and basic setup for testing purpose
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            canvas = new AppCanvas(500, 500);
            factory = new AppCommandFactory();
            program = new StoredProgram(canvas);
            parser = new AppParser(factory, program);
        }

        /// <summary>
        /// test method for validating test
        /// </summary>
        [TestMethod]
        public void MoveTo_UpdatesPenPositionCorrectly()
        {
            string code = "moveto 100,200";

            parser.Parse(code);
            program.Run();

            Assert.AreEqual(100, canvas.Xpos);
            Assert.AreEqual(200, canvas.Ypos);
        }
    }
}
