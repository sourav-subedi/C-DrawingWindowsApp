using System;
using System.Drawing;
using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// This class extends the canvas class from the library
    /// The umplemented method from the libray are implemented here
    /// </summary>
    public class AppCanvas : Canvas
    {
        private Bitmap bmp;
        private Graphics g;
        private Pen pen;
        private SolidBrush brush;
        private Color currentColour = Color.Black;

        private int _xpos;
        private int _ypos;

        /// <summary>
        /// it is used to initalize the canvas 
        /// </summary>
        /// <param name="width">the width of the canvas</param>
        /// <param name="height">the height of the canvas</param>
        public AppCanvas(int width, int height)
        {
            bmp = new Bitmap(Math.Max(1, width), Math.Max(1, height));
            g = Graphics.FromImage(bmp);
            pen = new Pen(Color.Black, 2);
            brush = new SolidBrush(Color.Black);
            Clear();
        }

        /// <summary>
        /// handlels the horizontal position of the cursor
        /// </summary>
        public override int Xpos
        {
            get => _xpos;
            set => _xpos = value;
        }
        /// <summary>
        /// handlels the verticle position of the cursor
        /// </summary>
        public override int Ypos
        {
            get => _ypos;
            set => _ypos = value;
        }

        /// <summary>
        /// handels the color of the pen
        /// </summary>
        public override object PenColour
        {
            get => currentColour;
            set
            {
                if (value is Color c)
                {
                    SetColour(c.R, c.G, c.B);
                }
                else if (value is int[] arr && arr.Length >= 3)
                {
                    SetColour(arr[0], arr[1], arr[2]);
                }
                else if (value is string s)
                {
                    
                    var parts = s.Split(',');
                    if (parts.Length >= 3 &&
                        int.TryParse(parts[0].Trim(), out int r) &&
                        int.TryParse(parts[1].Trim(), out int g) &&
                        int.TryParse(parts[2].Trim(), out int b))
                    {
                        SetColour(r, g, b);
                    }
                }
            }
        }

        /// <summary>
        /// draws the circle
        /// </summary>
        /// <param name="radius">the radius of the circle</param>
        /// <param name="filled">bool value to determine the fill of the circle </param>
        public override void Circle(int radius, bool filled)
        {
            if (radius <= 0) return;
            var rect = new Rectangle(_xpos - radius, _ypos - radius, radius * 2, radius * 2);
            if (filled)
                g.FillEllipse(brush, rect);
            else
                g.DrawEllipse(pen, rect);
        }

        /// <summary>
        /// draws the rectangle
        /// </summary>
        /// <param name="width">width of recatangle</param>
        /// <param name="height">height of rectangle</param>
        /// <param name="filled">bool value to determine the fill of the rectangle</param>
        public override void Rect(int width, int height, bool filled)
        {
            if (width <= 0 || height <= 0) return;
            if (filled)
                g.FillRectangle(brush, _xpos, _ypos, width, height);
            else
                g.DrawRectangle(pen, _xpos, _ypos, width, height);
        }
        /// <summary>
        /// handles the difference in spelling of the rect method
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="filled"></param>
        public override void Rectangle(int width, int height, bool filled)
        {
            Rect(width, height, filled);
        }

        /// <summary>
        /// draws the triangle
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public override void Tri(int width, int height)
        {
            if (width <= 0 || height == 0) return;

            Point p1 = new Point(_xpos, _ypos);
            Point p2 = new Point(_xpos + width, _ypos);
            Point p3 = new Point(_xpos + width / 2, _ypos - Math.Abs(height)); // up if height positive

            g.DrawPolygon(pen, new[] { p1, p2, p3 });
        }
        /// <summary>
        /// clears the canvas
        /// </summary>
        public override void Clear()
        {
            g.Clear(Color.White);
            //_xpos = 0;
            //_ypos = 0;
        }

        /// <summary>
        /// draws the line 
        /// </summary>
        /// <param name="x">horizontal position of the end of line</param>
        /// <param name="y">verticle position fo the end of line</param>
        public override void DrawTo(int x, int y)
        {
            g.DrawLine(pen, _xpos, _ypos, x, y);
            _xpos = x;
            _ypos = y;
        }

        /// <summary>
        /// returns the bitmap 
        /// </summary>
        /// <returns></returns>
        public override object getBitmap()
        {
            return bmp;
        }
        /// <summary>
        /// moves the pen to the desired position
        /// </summary>
        /// <param name="x">horizontal position of the pen</param>
        /// <param name="y">vertical postion of the pen</param>
        public override void MoveTo(int x, int y)
        {
            _xpos = x;
            _ypos = y;
        }
        /// <summary>
        /// resets the canvas and sets position of pen to 0,0
        /// </summary>
        public override void Reset()
        {
            _xpos = 0;
            _ypos = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public override void Set(int width, int height)
        {
            if (width <= 0) width = 1;
            if (height <= 0) height = 1;

            bmp?.Dispose();
            g?.Dispose();
            bmp = new Bitmap(width, height);
            g = Graphics.FromImage(bmp);
            Clear();
        }
        /// <summary>
        /// sets the color of the brush
        /// </summary>
        /// <param name="red">red value</param>
        /// <param name="green">green value</param>
        /// <param name="blue">blue value</param>
        public override void SetColour(int red, int green, int blue)
        {
            red = Math.Max(0, Math.Min(255, red));
            green = Math.Max(0, Math.Min(255, green));
            blue = Math.Max(0, Math.Min(255, blue));

            currentColour = Color.FromArgb(red, green, blue);

            float previousWidth = pen?.Width ?? 2;

            pen?.Dispose();
            brush?.Dispose();

            pen = new Pen(currentColour, previousWidth);
            brush = new SolidBrush(currentColour);
        }

        public override void WriteText(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            using (var f = new Font("Arial", 12))
            {
                g.DrawString(text, f, brush, _xpos, _ypos);
            }
        }
        /// <summary>
        /// writes the text in the canvas
        /// </summary>
        /// <param name="text">the text to be written</param>
        /// <param name="x">the horizontal position of the text</param>
        /// <param name="y">the vertical position of the text</param>
        public void WriteText(string text, int x, int y)
        {
            if (string.IsNullOrEmpty(text)) return;
            using (var f = new Font("Arial", 12))
            {
                g.DrawString(text, f, brush, x, y);
            }
        }

        /// <summary>
        /// used to dispose the resource after use
        /// </summary>
        public void DisposeResources()
        {
            pen?.Dispose();
            brush?.Dispose();
            g?.Dispose();
            bmp?.Dispose();
        }
    }
}
