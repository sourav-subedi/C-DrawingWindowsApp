using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSE;
using MYBooseApp;

namespace MyBooseAppUnitTest
{
    [TestClass]
    public class MoveToTests
    {
        private AppCanvas canvas;
        private StoredProgram program;
        private AppCommandFactory factory;
        private AppParser parser;

        [TestInitialize]
        public void Setup()
        {
            canvas = new AppCanvas(500, 500);
            factory = new AppCommandFactory();
            program = new StoredProgram(canvas);
            parser = new AppParser(factory, program);
        }

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
