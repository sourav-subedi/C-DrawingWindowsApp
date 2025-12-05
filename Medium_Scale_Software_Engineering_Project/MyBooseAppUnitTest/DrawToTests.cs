using BOOSE;
using MYBooseApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBooseAppUnitTest
{
    /// <summary>
    /// the test class to test drawto method
    /// </summary>
    [TestClass]
    public class DrawToTests
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
        public void DrawTo_UpdatesPenPositionCorrectly()
        {
            string code = "drawto 300,400";

            parser.Parse(code);
            program.Run();

            Assert.AreEqual(300, canvas.Xpos);
            Assert.AreEqual(400, canvas.Ypos);
        }
    }
}
