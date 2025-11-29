using BOOSE;
using System.Drawing;

namespace Medium_Scale_Software_Engineering_Project
{
    /// <summary>
    /// This Class is a extention of ICanvas Class which inmplements and overrites all the methods of the Icanvas Class
    /// </summary>
    internal class AppCanvas : Canvas
    {
        Bitmap bmp;
        Graphics g;
        Pen pen;
        Brush brush;
        int xpos, ypos;

        /// <summary>
        /// This is the constructor of the class which initializes all the necessary attribute of the class
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public AppCanvas(int width, int height)
        {
            bmp = new Bitmap(width, height);
            g = Graphics.FromImage(bmp);
            Clear();

            pen = new Pen(Color.Black, 2);
            brush = Brushes.Black;
        }

       
        public object PenColour
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawTo(int x, int y)
        {
            g.DrawLine(pen, xpos, ypos, x, y);
            xpos = x;
            ypos = y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="filled"></param>
        public void Circle(int radius, bool filled)
        {
            if (filled)
                g.FillEllipse(brush, xpos, ypos, radius * 2, radius * 2);
            else
                g.DrawEllipse(pen, xpos, ypos, radius * 2, radius * 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="filled"></param>
        public void Rect(int width, int height, bool filled)
        {
            if (filled)
                g.FillRectangle(brush, xpos, ypos, width, height);
            else
                g.DrawRectangle(pen, xpos, ypos, width, height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Tri(int width, int height)
        {
            Point p1 = new Point(xpos, ypos);
            Point p2 = new Point(xpos + width, ypos);
            Point p3 = new Point(xpos + width / 2, ypos - height);

            g.DrawPolygon(pen, new Point[] { p1, p2, p3 });
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            g.Clear(Color.White);
        }

        public void SetColour(int red, int green, int blue)
        {
            PenColour = Color.FromArgb(red, green, blue);
        }

        public void Set(int width, int height)
        {
            bmp = new Bitmap(width, height);
            g = Graphics.FromImage(bmp);
            Clear();
        }

        public void WriteText(string text)
        {
            g.DrawString(text, new Font("Arial", 14), brush, xpos, ypos);
        }

        public object getBitmap()
        {
            return bmp;
        }
    }
}
