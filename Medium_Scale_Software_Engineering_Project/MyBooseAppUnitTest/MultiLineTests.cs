using BOOSE;
using MYBooseApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBooseAppUnitTest
{
    [TestClass]
    public class MultiLineTests
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
        public void MultiLineProgram_ExecutesCorrectly()
        {
            string code = @"
                moveto 50,50
                circle 40
                drawto 200,200
                rect 100,50
            ";

            parser.Parse(code);
            program.Run();

            Assert.AreEqual(200, canvas.Xpos);
            Assert.AreEqual(200, canvas.Ypos);
        }
    }
}
