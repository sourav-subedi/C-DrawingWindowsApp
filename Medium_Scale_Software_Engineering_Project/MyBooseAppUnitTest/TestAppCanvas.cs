using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using BOOSE;

namespace UnitTests
{
    /// <summary>
    /// Test implementation of ICanvas for unit testing.
    /// Provides an independent canvas instance per test (no singleton).
    /// Tracks drawing operations (circle, pen color, text) for easy assertions.
    /// Thread-safe for parallel test execution.
    /// </summary>
    public class TestAppCanvas : ICanvas, IDisposable
    {
        /// <summary>
        /// The underlying bitmap used for all drawing operations.
        /// </summary>
        private Bitmap CanvasBitmap;

        /// <summary>
        /// Graphics context for rendering on the bitmap.
        /// </summary>
        private Graphics graphics;

        /// <summary>
        /// Current X position of the drawing cursor.
        /// </summary>
        private int xPos;

        /// <summary>
        /// Current Y position of the drawing cursor.
        /// </summary>
        private int yPos;

        /// <summary>
        /// Current pen width for drawing operations.
        /// </summary>
        private int PenWidth = 1;

        /// <summary>
        /// Current pen used for lines, outlines, and text color.
        /// </summary>
        private Pen Pen;

        /// <summary>
        /// Stores the radius of the most recently drawn circle (for test assertions).
        /// </summary>
        private int lastCircleRadius = -1;

        /// <summary>
        /// Indicates whether the most recently drawn circle was filled.
        /// </summary>
        private bool lastCircleFilled = false;

        /// <summary>
        /// Stores the most recently set pen color (for test assertions).
        /// </summary>
        private Color lastPenColor = Color.Black;

        /// <summary>
        /// Collects all text written via WriteText (for output verification).
        /// </summary>
        private readonly List<string> outputTexts = new List<string>();

        /// <summary>
        /// Initializes a new independent canvas instance for unit testing.
        /// Sets up a white background with anti-aliased graphics.
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

        /// <summary>
        /// Gets or sets the current X position of the drawing cursor.
        /// </summary>
        public int Xpos { get => xPos; set => xPos = value; }

        /// <summary>
        /// Gets or sets the current Y position of the drawing cursor.
        /// </summary>
        public int Ypos { get => yPos; set => yPos = value; }

        /// <summary>
        /// Gets or sets the current pen color.
        /// </summary>
        public object PenColour { get => Pen.Color; set => Pen.Color = (Color)value; }

        /// <summary>
        /// Draws a circle centered at the current cursor position.
        /// </summary>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="filled">If true, the circle is filled with the current pen color.</param>
        public void Circle(int radius, bool filled = false)
        {
            int x = xPos - radius;
            int y = yPos - radius;

            if (filled)
                graphics.FillEllipse(new SolidBrush(Pen.Color), x, y, radius * 2, radius * 2);
            else
                graphics.DrawEllipse(Pen, x, y, radius * 2, radius * 2);

            // Store for test assertions
            lastCircleRadius = radius;
            lastCircleFilled = filled;
        }

        /// <summary>
        /// Resets the canvas state: cursor to (0,0), pen to black width 1, clears to white.
        /// Also resets test trackers.
        /// </summary>
        public void Reset()
        {
            xPos = 0;
            yPos = 0;
            Pen = new Pen(Color.Black, PenWidth);
            graphics.Clear(Color.White);

            // Reset test trackers
            lastCircleRadius = -1;
            lastCircleFilled = false;
            lastPenColor = Color.Black;
            outputTexts.Clear();
        }

        /// <summary>
        /// Clears the canvas to white without resetting cursor or pen.
        /// </summary>
        public void Clear()
        {
            graphics.Clear(Color.White);
        }

        /// <summary>
        /// Draws a line from the current cursor position to the specified coordinates.
        /// Updates cursor position to the endpoint.
        /// </summary>
        public void DrawTo(int x, int y)
        {
            graphics.DrawLine(Pen, xPos, yPos, x, y);
            xPos = x;
            yPos = y;
        }

        /// <summary>
        /// Moves the cursor to the specified coordinates without drawing.
        /// </summary>
        public void MoveTo(int x, int y)
        {
            xPos = x;
            yPos = y;
        }

        /// <summary>
        /// Draws a rectangle starting at the current cursor position.
        /// </summary>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="filled">If true, the rectangle is filled with the current pen color.</param>
        public void Rect(int width, int height, bool filled)
        {
            if (filled)
                graphics.FillRectangle(new SolidBrush(Pen.Color), xPos, yPos, width, height);
            else
                graphics.DrawRectangle(Pen, xPos, yPos, width, height);
        }

        /// <summary>
        /// Resizes the canvas to the specified dimensions and clears it to white.
        /// </summary>
        /// <param name="width">New width.</param>
        /// <param name="height">New height.</param>
        public void Set(int width, int height)
        {
            graphics?.Dispose();
            CanvasBitmap?.Dispose();

            CanvasBitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(CanvasBitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.White);
        }

        /// <summary>
        /// Sets the pen color using RGB values.
        /// </summary>
        /// <param name="red">Red component (0-255).</param>
        /// <param name="green">Green component (0-255).</param>
        /// <param name="blue">Blue component (0-255).</param>
        public void SetColour(int red, int green, int blue)
        {
            Pen.Color = Color.FromArgb(red, green, blue);
            lastPenColor = Pen.Color; // Store for test assertions
        }

        /// <summary>
        /// Draws an unfilled triangle with base at the current cursor position.
        /// </summary>
        /// <param name="width">Base width of the triangle.</param>
        /// <param name="height">Height of the triangle.</param>
        public void Tri(int width, int height)
        {
            Point[] points = new Point[3];
            points[0] = new Point(xPos + width / 2, yPos);
            points[1] = new Point(xPos, yPos + height);
            points[2] = new Point(xPos + width, yPos + height);
            graphics.DrawPolygon(Pen, points);
        }

        /// <summary>
        /// Draws text at the current cursor position using the current pen color.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        public void WriteText(string text)
        {
            graphics.DrawString(text, new Font("Arial", 12), new SolidBrush(Pen.Color), xPos, yPos);
            outputTexts.Add(text); // Store for test assertions
        }

        /// <summary>
        /// Sets the pen width for subsequent drawing operations.
        /// </summary>
        /// <param name="size">The new pen width.</param>
        public void penSize(int size)
        {
            PenWidth = size;
            Pen.Width = size;
        }

        /// <summary>
        /// Returns the current canvas bitmap.
        /// </summary>
        /// <returns>The Bitmap object containing all drawn content.</returns>
        public object getBitmap()
        {
            return CanvasBitmap;
        }

        /// <summary>
        /// Releases all resources used by the canvas.
        /// Should be called when the canvas is no longer needed.
        /// </summary>
        public void Dispose()
        {
            graphics?.Dispose();
            Pen?.Dispose();
            CanvasBitmap?.Dispose();
        }

        // Test-specific getters for assertions

        /// <summary>
        /// Returns the radius of the most recently drawn circle (-1 if none).
        /// </summary>
        public int GetLastCircleRadius() => lastCircleRadius;

        /// <summary>
        /// Returns whether the most recently drawn circle was filled.
        /// </summary>
        public bool GetLastCircleFilled() => lastCircleFilled;

        /// <summary>
        /// Returns the most recently set pen color.
        /// </summary>
        public Color GetLastPenColor() => lastPenColor;

        /// <summary>
        /// Returns all text written via WriteText, joined by newlines.
        /// </summary>
        public string GetOutputText() => string.Join("\n", outputTexts);
    }
}