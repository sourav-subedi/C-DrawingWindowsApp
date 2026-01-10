using BOOSE;
using MYBooseApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyBooseAppUnitTest
{
    /// <summary>
    /// Test class to validate execution of multiline commands
    /// </summary>
    [TestClass]
    public class MultiLineTests
    {
        private AppCanvas canvas;
        private StoredProgram program;
        private AppCommandFactory factory;
        private AppParser parser;

        /// <summary>
        /// Initial setup for the tests
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
        /// Test method for executing multiple commands sequentially
        /// </summary>
        [TestMethod]
        public void MultiLineProgram_ExecutesCorrectly()
        {
            string code = @"
                moveto 50,50
                circle 40
                drawto 200,200
                rect 100,50
            ";

           
            parser.ParseProgram(code);
            program.Run();

            // Check final pen position
            Assert.AreEqual(200, canvas.Xpos);
            Assert.AreEqual(200, canvas.Ypos);
        }
    }
}
