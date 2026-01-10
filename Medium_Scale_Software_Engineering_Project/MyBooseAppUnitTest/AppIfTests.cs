using BOOSE;
using MYBooseApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace MyBooseAppUnitTest
{
    [TestClass]
    public class AppIfTests
    {
        [TestMethod]
        public void AppIf_InstantiatesWithoutError()
        {
            var appIf = new AppIf();
            Assert.IsNotNull(appIf, "AppIf instance should not be null");
        }

        [TestMethod]
        public void AppIf_ResetsStaticCounter_ReflectionSafe()
        {
            // Try to find any static int field in If class
            var ifType = typeof(If);
            var fields = ifType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
            int counterFieldCount = 0;

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(int))
                {
                    counterFieldCount++;
                    field.SetValue(null, 0); // reset it
                }
            }

            var appIf = new AppIf(); // should not crash

            Assert.IsNotNull(appIf);
            Assert.IsTrue(counterFieldCount > 0, "There should be at least one static int field in If class");
        }

        [TestMethod]
        public void AppIf_CanParseFromProgramText()
        {
            var canvas = new MockCanvas();
            var program = new StoredProgram(canvas);
            var factory = new AppCommandFactory();
            var parser = new AppParser(factory, program);

            // minimal if program text
            string code = @"
                int x 5
                if x = 5
                    moveto 10,10
                end
            ";

            try
            {
                parser.ParseProgram(code); // parse text instead of raw AppIf
                program.Run(); // run program
            }
            catch (Exception ex)
            {
                Assert.Fail("Parsing and running an if block from text should not throw: " + ex.Message);
            }
        }
    }
}
