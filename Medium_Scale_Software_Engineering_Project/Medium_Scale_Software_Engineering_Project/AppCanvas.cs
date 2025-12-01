using System;
using System.Drawing;
using BOOSE;

namespace MYBooseApp
{
    internal class AppCanvas : Canvas
    {
        private Bitmap bmp;
        private Graphics g;
        private Pen pen;
        private SolidBrush brush;
        private Color currentColour = Color.Black;

        private int _xpos;
        private int _ypos;

        public AppCanvas(int width, int height)
        {
            bmp = new Bitmap(Math.Max(1, width), Math.Max(1, height));
            g = Graphics.FromImage(bmp);
            pen = new Pen(Color.Black, 2);
            brush = new SolidBrush(Color.Black);
            Clear();
        }

        public override int Xpos
        {
            get => _xpos;
            set => _xpos = value;
        }

        public override int Ypos
        {
            get => _ypos;
            set => _ypos = value;
        }

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

        
        public override void Circle(int radius, bool filled)
        {
            if (radius <= 0) return;
            var rect = new Rectangle(_xpos - radius, _ypos - radius, radius * 2, radius * 2);
            if (filled)
                g.FillEllipse(brush, rect);
            else
                g.DrawEllipse(pen, rect);
        }

        
        public override void Rect(int width, int height, bool filled)
        {
            if (width <= 0 || height <= 0) return;
            if (filled)
                g.FillRectangle(brush, _xpos, _ypos, width, height);
            else
                g.DrawRectangle(pen, _xpos, _ypos, width, height);
        }
        public override void Rectangle(int width, int height, bool filled)
        {
            Rect(width, height, filled);
        }

        public override void Tri(int width, int height)
        {
            if (width <= 0 || height == 0) return;

            Point p1 = new Point(_xpos, _ypos);
            Point p2 = new Point(_xpos + width, _ypos);
            Point p3 = new Point(_xpos + width / 2, _ypos - Math.Abs(height)); // up if height positive

            g.DrawPolygon(pen, new[] { p1, p2, p3 });
        }

        public override void Clear()
        {
            g.Clear(Color.White);
            _xpos = 0;
            _ypos = 0;
        }

        public override void DrawTo(int x, int y)
        {
            g.DrawLine(pen, _xpos, _ypos, x, y);
            _xpos = x;
            _ypos = y;
        }

        public override object getBitmap()
        {
            return bmp;
        }
        public override void MoveTo(int x, int y)
        {
            _xpos = x;
            _ypos = y;
        }
        public override void Reset()
        {
            _xpos = 0;
            _ypos = 0;
        }
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
        public void WriteText(string text, int x, int y)
        {
            if (string.IsNullOrEmpty(text)) return;
            using (var f = new Font("Arial", 12))
            {
                g.DrawString(text, f, brush, x, y);
            }
        }

        public void DisposeResources()
        {
            pen?.Dispose();
            brush?.Dispose();
            g?.Dispose();
            bmp?.Dispose();
        }
    }
}
