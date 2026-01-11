using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using BOOSE;

namespace MyBooseAppUnitTest
{
    /// <summary>
    /// Test implementation of <see cref="ICanvas"/> for unit testing.
    /// Provides an independent canvas instance per test.
    /// Tracks drawing operations (circle, pen color, text) for assertions.
    /// Thread-safe when each test uses its own instance.
    /// </summary>
    public class TestAppCanvas : ICanvas, IDisposable
    {
        private Bitmap CanvasBitmap;
        private Graphics graphics;
        private int xPos;
        private int yPos;
        private int PenWidth = 1;
        private Pen Pen;
        private int lastCircleRadius = -1;
        private bool lastCircleFilled = false;
        private Color lastPenColor = Color.Black;
        private readonly List<string> outputTexts = new List<string>();

        /// <summary>
        /// Initializes a new canvas with the specified width and height.
        /// </summary>
        /// <param name="xsize">Width of the canvas in pixels.</param>
        /// <param name="ysize">Height of the canvas in pixels.</param>
        public TestAppCanvas(int xsize, int ysize)
        {
            CanvasBitmap = new Bitmap(xsize, ysize);
            graphics = Graphics.FromImage(CanvasBitmap);
            xPos = 0;
            yPos = 0;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Pen = new Pen(Color.Black, PenWidth);
            graphics.Clear(Color.White);
        }

        /// <summary>Gets or sets the current X position of the drawing cursor.</summary>
        public int Xpos { get => xPos; set => xPos = value; }

        /// <summary>Gets or sets the current Y position of the drawing cursor.</summary>
        public int Ypos { get => yPos; set => yPos = value; }

        /// <summary>Gets or sets the current pen color.</summary>
        public object PenColour { get => Pen.Color; set => Pen.Color = (Color)value; }

        /// <summary>Draws a circle centered at the current cursor position.</summary>
        /// <param name="radius">The circle radius in pixels.</param>
        /// <param name="filled">If true, fills the circle with the current pen color.</param>
        public void Circle(int radius, bool filled = false)
        {
            int x = xPos - radius;
            int y = yPos - radius;

            if (filled)
                graphics.FillEllipse(new SolidBrush(Pen.Color), x, y, radius * 2, radius * 2);
            else
                graphics.DrawEllipse(Pen, x, y, radius * 2, radius * 2);

            lastCircleRadius = radius;
            lastCircleFilled = filled;
        }

        /// <summary>Resets the canvas: clears to white, resets cursor and pen, and clears test trackers.</summary>
        public void Reset()
        {
            xPos = 0;
            yPos = 0;
            Pen = new Pen(Color.Black, PenWidth);
            graphics.Clear(Color.White);
            lastCircleRadius = -1;
            lastCircleFilled = false;
            lastPenColor = Color.Black;
            outputTexts.Clear();
        }

        /// <summary>Clears the canvas to white without modifying cursor or pen.</summary>
        public void Clear() => graphics.Clear(Color.White);

        /// <summary>Draws a line from the current cursor position to the specified point and updates the cursor.</summary>
        public void DrawTo(int x, int y)
        {
            graphics.DrawLine(Pen, xPos, yPos, x, y);
            xPos = x;
            yPos = y;
        }

        /// <summary>Moves the cursor to the specified coordinates without drawing.</summary>
        public void MoveTo(int x, int y)
        {
            xPos = x;
            yPos = y;
        }

        /// <summary>Draws a rectangle starting at the current cursor position.</summary>
        /// <param name="width">Rectangle width.</param>
        /// <param name="height">Rectangle height.</param>
        /// <param name="filled">If true, fills the rectangle with the current pen color.</param>
        public void Rect(int width, int height, bool filled)
        {
            if (filled)
                graphics.FillRectangle(new SolidBrush(Pen.Color), xPos, yPos, width, height);
            else
                graphics.DrawRectangle(Pen, xPos, yPos, width, height);
        }

        /// <summary>Resizes the canvas to the specified dimensions and clears it to white.</summary>
        public void Set(int width, int height)
        {
            graphics?.Dispose();
            CanvasBitmap?.Dispose();
            CanvasBitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(CanvasBitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.White);
        }

        /// <summary>Sets the pen color using RGB values.</summary>
        public void SetColour(int red, int green, int blue)
        {
            Pen.Color = Color.FromArgb(red, green, blue);
            lastPenColor = Pen.Color;
        }

        /// <summary>Draws an unfilled triangle with base at the current cursor position.</summary>
        /// <param name="width">Base width.</param>
        /// <param name="height">Triangle height.</param>
        public void Tri(int width, int height)
        {
            Point[] points = new Point[3]
            {
                new Point(xPos + width / 2, yPos),
                new Point(xPos, yPos + height),
                new Point(xPos + width, yPos + height)
            };
            graphics.DrawPolygon(Pen, points);
        }

        /// <summary>Draws text at the current cursor position using the current pen color.</summary>
        /// <param name="text">Text to draw.</param>
        public void WriteText(string text)
        {
            graphics.DrawString(text, new Font("Arial", 12), new SolidBrush(Pen.Color), xPos, yPos);
            outputTexts.Add(text);
        }

        /// <summary>Sets the pen width for subsequent drawing operations.</summary>
        /// <param name="size">Pen width in pixels.</param>
        public void penSize(int size)
        {
            PenWidth = size;
            Pen.Width = size;
        }

        /// <summary>Returns the bitmap representing the current canvas.</summary>
        public object getBitmap() => CanvasBitmap;

        /// <summary>Disposes all resources used by the canvas.</summary>
        public void Dispose()
        {
            graphics?.Dispose();
            Pen?.Dispose();
            CanvasBitmap?.Dispose();
        }

        /// <summary>Returns the radius of the last drawn circle (-1 if none).</summary>
        public int GetLastCircleRadius() => lastCircleRadius;

        /// <summary>Returns whether the last drawn circle was filled.</summary>
        public bool GetLastCircleFilled() => lastCircleFilled;

        /// <summary>Returns the most recently set pen color.</summary>
        public Color GetLastPenColor() => lastPenColor;

        /// <summary>Returns all text written to the canvas, joined by newlines.</summary>
        public string GetOutputText() => string.Join("\n", outputTexts);
    }
}
