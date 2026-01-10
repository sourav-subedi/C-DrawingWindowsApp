using BOOSE;

namespace MyBooseAppUnitTest
{
    /// <summary>
    /// Mock canvas used for unit testing BOOSE commands.
    /// Implements full ICanvas contract.
    /// </summary>
    public class MockCanvas : ICanvas
    {
        public int Xpos { get; set; }
        public int Ypos { get; set; }

        // IMPORTANT: must be object, not Color
        public object PenColour { get; set; }

        public void MoveTo(int x, int y)
        {
            Xpos = x;
            Ypos = y;
        }

        public void DrawTo(int x, int y)
        {
            Xpos = x;
            Ypos = y;
        }

        public void Clear() { }

        public void Reset()
        {
            Xpos = 0;
            Ypos = 0;
        }

        public void Circle(int radius, bool filled) { }

        public void Rect(int width, int height, bool filled) { }

        public void Tri(int width, int height) { }

        public void WriteText(string text) { }

        public void Set(int x, int y)
        {
            Xpos = x;
            Ypos = y;
        }

        public void SetColour(int red, int green, int blue)
        {
            // store colour as anonymous object
            PenColour = new { red, green, blue };
        }

        public object getBitmap()
        {
            return null; // not required for unit testing
        }
    }
}
