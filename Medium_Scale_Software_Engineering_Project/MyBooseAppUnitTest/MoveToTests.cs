using BOOSE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MYBooseApp;

namespace MyBooseAppUnitTest
{
    [TestClass]
    public class MoveToTests
    {
        [TestMethod]
        public void MoveTo_ShouldUpdatePenPosition()
        {
            // Arrange
            var canvas = new AppCanvas(500, 500);

            // Act
            canvas.MoveTo(50, 100);

            // Assert
            Assert.AreEqual(50, canvas.Xpos);
            Assert.AreEqual(100, canvas.Ypos);
        }
    }
}
