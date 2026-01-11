using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BOOSE;

namespace MYBooseApp
{
    /// <summary>
    /// Implements the ICanvas interface to provide drawing functionality.
    /// This class uses the Singleton design pattern to guarantee that
    /// only one canvas instance is created and shared across the application.
    /// </summary>
    public class AppCanvas : ICanvas
    {
        /// <summary>
        /// Holds the single shared instance of the AppCanvas.
        /// </summary>
        private static AppCanvas _instance;

        /// <summary>
        /// Synchronization object used to ensure thread-safe
        /// creation of the singleton instance.
        /// </summary>
        private static readonly object _lock = new object();

        private Bitmap CanvasBitmap;
        private Graphics graphics;
        private int xPos, yPos, PenWidth = 1;
        private Pen Pen;

        /// <summary>
        /// Reference to the PictureBox control responsible for displaying the canvas.
        /// </summary>
        private PictureBox displayControl;

        /// <summary>
        /// Initializes a new AppCanvas instance.
        /// The constructor is private to prevent external instantiation
        /// and enforce the Singleton pattern.
        /// </summary>
        /// <param name="xsize">Initial width of the canvas</param>
        /// <param name="ysize">Initial height of the canvas</param>
        private AppCanvas(int xsize, int ysize)
        {
            CanvasBitmap = new Bitmap(xsize, ysize);
            graphics = Graphics.FromImage(CanvasBitmap);
            xPos = 0;
            yPos = 0;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Pen = new Pen(Color.Black, PenWidth);
        }

        /// <summary>
        /// Returns the global AppCanvas instance, creating it if necessary.
        /// </summary>
        /// <param name="width">Canvas width (used only on first creation)</param>
        /// <param name="height">Canvas height (used only on first creation)</param>
        /// <returns>The singleton AppCanvas instance</returns>
        public static AppCanvas Instance(int width = 1280, int height = 1080)
        {
            lock (_lock)
            {
                if (_instance == null)
                    _instance = new AppCanvas(width, height);
                return _instance;
            }
        }

        /// <summary>
        /// Associates the canvas bitmap with a PictureBox for rendering.
        /// </summary>
        /// <param name="pictureBox">PictureBox used to display the canvas</param>
        public void LinkToPictureBox(PictureBox pictureBox)
        {
            displayControl = pictureBox;
            displayControl.Image = CanvasBitmap;
        }

        /// <summary>
        /// Forces the display control to repaint and show the latest drawing updates.
        /// </summary>
        private void RefreshDisplay()
        {
            displayControl?.Invalidate();
        }

        /// <summary>
        /// Gets or sets the current horizontal position of the drawing cursor.
        /// </summary>
        public int Xpos { get => xPos; set => xPos = value; }

        /// <summary>
        /// Gets or sets the current vertical position of the drawing cursor.
        /// </summary>
        public int Ypos { get => yPos; set => yPos = value; }

        /// <summary>
        /// Gets or sets the current pen colour.
        /// </summary>
        public object PenColour { get => Pen.Color; set => Pen.Color = (Color)value; }

        /// <summary>
        /// Draws a circle centred at the current cursor position.
        /// </summary>
        /// <param name="radius">Radius of the circle</param>
        /// <param name="filled">Indicates whether the circle should be filled</param>
        public void Circle(int radius, bool filled = false)
        {
            int x = xPos - radius;
            int y = yPos - radius;

            if (filled)
                graphics.FillEllipse(new SolidBrush(Pen.Color), x, y, radius * 2, radius * 2);
            else
                graphics.DrawEllipse(Pen, x, y, radius * 2, radius * 2);

            RefreshDisplay();
        }

        /// <summary>
        /// Resets the canvas to its initial state, clearing all drawings
        /// and restoring default cursor and pen settings.
        /// </summary>
        public void Reset()
        {
            xPos = 0;
            yPos = 0;
            Pen = new Pen(Color.Black, PenWidth);
            graphics.Clear(Color.White);
            RefreshDisplay();
        }

        /// <summary>
        /// Clears all drawings from the canvas.
        /// </summary>
        public void Clear()
        {
            graphics.Clear(Color.White);
            RefreshDisplay();
        }

        /// <summary>
        /// Draws a line from the current cursor position to the specified point.
        /// </summary>
        /// <param name="x">Target X coordinate</param>
        /// <param name="y">Target Y coordinate</param>
        public void DrawTo(int x, int y)
        {
            graphics.DrawLine(Pen, xPos, yPos, x, y);
            xPos = x;
            yPos = y;
            RefreshDisplay();
        }

        /// <summary>
        /// Moves the drawing cursor to the specified location without drawing.
        /// </summary>
        /// <param name="x">New X coordinate</param>
        /// <param name="y">New Y coordinate</param>
        public void MoveTo(int x, int y)
        {
            xPos = x;
            yPos = y;
        }

        /// <summary>
        /// Draws a rectangle using the current cursor position as the top-left corner.
        /// </summary>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        /// <param name="filled">Indicates whether the rectangle should be filled</param>
        public void Rect(int width, int height, bool filled)
        {
            if (filled)
                graphics.FillRectangle(new SolidBrush(Pen.Color), xPos, yPos, width, height);
            else
                graphics.DrawRectangle(Pen, xPos, yPos, width, height);

            RefreshDisplay();
        }

        /// <summary>
        /// Resizes the canvas and reinitializes the drawing surface.
        /// </summary>
        /// <param name="width">New canvas width</param>
        /// <param name="height">New canvas height</param>
        public void Set(int width, int height)
        {
            CanvasBitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(CanvasBitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (displayControl != null)
                displayControl.Image = CanvasBitmap;

            RefreshDisplay();
        }

        /// <summary>
        /// Sets the pen colour using individual RGB components.
        /// </summary>
        /// <param name="red">Red value (0–255)</param>
        /// <param name="green">Green value (0–255)</param>
        /// <param name="blue">Blue value (0–255)</param>
        public void SetColour(int red, int green, int blue)
        {
            Pen.Color = Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// Draws an outlined triangle at the current cursor position.
        /// </summary>
        /// <param name="width">Base width of the triangle</param>
        /// <param name="height">Height of the triangle</param>
        public void Tri(int width, int height)
        {
            Point[] points = new Point[3];
            points[0] = new Point(xPos + width / 2, yPos);
            points[1] = new Point(xPos, yPos + height);
            points[2] = new Point(xPos + width, yPos + height);

            graphics.DrawPolygon(Pen, points);
            RefreshDisplay();
        }

        /// <summary>
        /// Renders text at the current cursor position.
        /// </summary>
        /// <param name="text">Text to be drawn</param>
        public void WriteText(string text)
        {
            graphics.DrawString(
                text,
                new Font("Arial", 12),
                new SolidBrush(Pen.Color),
                xPos,
                yPos);

            RefreshDisplay();
        }

        /// <summary>
        /// Updates the pen thickness used for drawing operations.
        /// </summary>
        /// <param name="size">New pen width</param>
        public void penSize(int size)
        {
            PenWidth = size;
            Pen.Width = size;
        }

        /// <summary>
        /// Retrieves the current bitmap representing the canvas contents.
        /// </summary>
        /// <returns>The canvas bitmap</returns>
        public object getBitmap()
        {
            return CanvasBitmap;
        }

        /// <summary>
        /// Resets the singleton instance, allowing a new canvas
        /// to be created on the next request.
        /// </summary>
        public static void ResetInstance()
        {
            _instance = null;
        }
    }
}
