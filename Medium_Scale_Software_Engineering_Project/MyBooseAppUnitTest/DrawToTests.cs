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
        [TestMethod]
        public void DrawTo_ShouldUpdatePenPosition()
        {
            // Arrange
            var canvas = new AppCanvas(500, 500);

            // Act
            canvas.DrawTo(50, 100);

            // Assert
            Assert.AreEqual(50, canvas.Xpos);
            Assert.AreEqual(100, canvas.Ypos);
        }
    }
}
