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
        public int xpos, ypos;

        public AppCanvas(int width, int height)
        {
            bmp = new Bitmap(width, height);
            g = Graphics.FromImage(bmp);
            pen = new Pen(Color.Black, 2);
            brush = new SolidBrush(Color.Black);
            Clear();
        }

       
        public override void MoveTo(int x, int y)
        {
            xpos = x;
            ypos = y;
        }

        public override void DrawTo(int x, int y)
        {
            g.DrawLine(pen, xpos, ypos, x, y);
            xpos = x;
            ypos = y;
        }

        public override void Circle(int radius, bool filled)
        {
            Rectangle r = new Rectangle(xpos - radius, ypos - radius, radius * 2, radius * 2);
            if (filled)
                g.FillEllipse(brush, r);
            else
                g.DrawEllipse(pen, r);
        }

        public override void Rect(int width, int height, bool filled)
        {
            if (filled)
                g.FillRectangle(brush, xpos, ypos, width, height);
            else
                g.DrawRectangle(pen, xpos, ypos, width, height);
        }

        public override void Clear()
        {
            g.Clear(Color.White);
            xpos = 0;
            ypos = 0;
        }

        public override object PenColour
        {
            get => pen.Color;
            set
            {
                if (value is Color c)
                {
                    pen = new Pen(c, 2);
                    brush = new SolidBrush(c);
                }
            }
        }

        public override object getBitmap() => bmp;

        
        public void Set(int width, int height)
        {
            bmp?.Dispose();
            g?.Dispose();
            bmp = new Bitmap(width, height);
            g = Graphics.FromImage(bmp);
            Clear();
        }

        public void WriteText(string text, int x, int y)
        {
            g.DrawString(text, new Font("Arial", 12), brush, x, y);
        }
    }
}