using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using BOOSE;

namespace BOOSEWebAPI
{
    /// <summary>
    /// WebCanvas class implements the ICanvas interface for ASP.NET Core Web API.
    /// Unlike AppCanvas, this does NOT use the Singleton pattern since each HTTP request
    /// should have its own independent canvas instance for thread safety.
    /// Generates server-side images that can be converted to Base64 for web transmission.
    /// </summary>
    public class WebCanvas : ICanvas
    {
        // Canvas bitmap for drawing operations
        private Bitmap CanvasBitmap;

        // Graphics object for drawing on the bitmap
        private Graphics graphics;

        // Current cursor position
        private int xPos, yPos;

        // Current pen width
        private int PenWidth = 1;

        // Pen used for drawing operations
        private Pen Pen;

        /// <summary>
        /// Public constructor creates a new canvas instance for each request.
        /// No singleton pattern - allows multiple concurrent canvases.
        /// </summary>
        /// <param name="xsize">Width of the canvas</param>
        /// <param name="ysize">Height of the canvas</param>
        public WebCanvas(int xsize, int ysize)
        {
            // Create new bitmap with specified dimensions
            CanvasBitmap = new Bitmap(xsize, ysize);

            // Get graphics object from bitmap for drawing
            graphics = Graphics.FromImage(CanvasBitmap);

            // Initialize cursor position at origin
            xPos = 0;
            yPos = 0;

            // Enable anti-aliasing for smooth drawing
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Initialize pen with black color and default width
            Pen = new Pen(Color.Black, PenWidth);

            // Start with white background
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
        /// Gets or sets the pen colour.
        /// </summary>
        public object PenColour { get => Pen.Color; set => Pen.Color = (Color)value; }

        /// <summary>
        /// Draws a circle using the current cursor position as the center.
        /// </summary>
        /// <param name="radius">Radius of the circle</param>
        /// <param name="filled">Whether the circle is filled</param>
        public void Circle(int radius, bool filled = false)
        {
            // Calculate top-left corner for bounding rectangle
            int x = xPos - radius;
            int y = yPos - radius;

            // Draw filled or outlined circle
            if (filled)
                graphics.FillEllipse(new SolidBrush(Pen.Color), x, y, radius * 2, radius * 2);
            else
                graphics.DrawEllipse(Pen, x, y, radius * 2, radius * 2);
        }

        /// <summary>
        /// Clears the canvas and resets cursor and pen state.
        /// </summary>
        public void Reset()
        {
            // Reset cursor to origin
            xPos = 0;
            yPos = 0;

            // Reset pen to default (black, width 1)
            Pen = new Pen(Color.Black, PenWidth);

            // Clear canvas to white
            graphics.Clear(Color.White);
        }

        /// <summary>
        /// Clears the canvas by filling it with white colour.
        /// </summary>
        public void Clear()
        {
            graphics.Clear(Color.White);
        }

        /// <summary>
        /// Draws a line from the current cursor position to the given coordinates.
        /// Updates cursor position to the end point.
        /// </summary>
        /// <param name="x">Destination X coordinate</param>
        /// <param name="y">Destination Y coordinate</param>
        public void DrawTo(int x, int y)
        {
            // Draw line from current position to new position
            graphics.DrawLine(Pen, xPos, yPos, x, y);

            // Update cursor position to end of line
            xPos = x;
            yPos = y;
        }

        /// <summary>
        /// Moves the drawing cursor to the specified coordinates without drawing.
        /// </summary>
        /// <param name="x">New X coordinate</param>
        /// <param name="y">New Y coordinate</param>
        public void MoveTo(int x, int y)
        {
            xPos = x;
            yPos = y;
        }

        /// <summary>
        /// Draws a rectangle at the current cursor position.
        /// </summary>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        /// <param name="filled">Whether the rectangle is filled</param>
        public void Rect(int width, int height, bool filled)
        {
            // Draw filled or outlined rectangle
            if (filled)
                graphics.FillRectangle(new SolidBrush(Pen.Color), xPos, yPos, width, height);
            else
                graphics.DrawRectangle(Pen, xPos, yPos, width, height);
        }

        /// <summary>
        /// Sets a new canvas size and resets the drawing surface.
        /// </summary>
        /// <param name="width">New canvas width</param>
        /// <param name="height">New canvas height</param>
        public void Set(int width, int height)
        {
            // Create new bitmap with new dimensions
            CanvasBitmap = new Bitmap(width, height);

            // Get new graphics object
            graphics = Graphics.FromImage(CanvasBitmap);

            // Re-enable anti-aliasing
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Clear to white
            graphics.Clear(Color.White);
        }

        /// <summary>
        /// Sets the pen colour using RGB values.
        /// </summary>
        /// <param name="red">Red component (0–255)</param>
        /// <param name="green">Green component (0–255)</param>
        /// <param name="blue">Blue component (0–255)</param>
        public void SetColour(int red, int green, int blue)
        {
            Pen.Color = Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// Draws an unfilled triangle at the current cursor position.
        /// </summary>
        /// <param name="width">Width of the triangle</param>
        /// <param name="height">Height of the triangle</param>
        public void Tri(int width, int height)
        {
            // Define triangle points (top center, bottom left, bottom right)
            Point[] points = new Point[3];
            points[0] = new Point(xPos + width / 2, yPos);           // Top
            points[1] = new Point(xPos, yPos + height);              // Bottom left
            points[2] = new Point(xPos + width, yPos + height);      // Bottom right

            // Draw triangle outline
            graphics.DrawPolygon(Pen, points);
        }

        /// <summary>
        /// Draws text at the current cursor position.
        /// </summary>
        /// <param name="text">Text to draw</param>
        public void WriteText(string text)
        {
            // Draw text with Arial font, size 12, using current pen color
            graphics.DrawString(text, new Font("Arial", 12),
                new SolidBrush(Pen.Color), xPos, yPos);
        }

        /// <summary>
        /// Sets the pen width used for drawing.
        /// </summary>
        /// <param name="size">New pen size</param>
        public void penSize(int size)
        {
            PenWidth = size;
            Pen.Width = size;
        }

        /// <summary>
        /// Returns the current canvas bitmap.
        /// </summary>
        /// <returns>Canvas bitmap</returns>
        public object getBitmap()
        {
            return CanvasBitmap;
        }

        /// <summary>
        /// Converts the canvas bitmap to a Base64-encoded PNG string for web transmission.
        /// This is the key method for ASP.NET Web API - sends image to frontend.
        /// </summary>
        /// <returns>Base64-encoded PNG image string</returns>
        public string GetBase64Image()
        {
            using (var ms = new MemoryStream())
            {
                // Save bitmap to memory stream as PNG
                CanvasBitmap.Save(ms, ImageFormat.Png);

                // Convert to Base64 string for JSON response
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// Converts the canvas bitmap to a byte array for alternative transmission methods.
        /// </summary>
        /// <returns>PNG image as byte array</returns>
        public byte[] GetImageBytes()
        {
            using (var ms = new MemoryStream())
            {
                CanvasBitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Disposes graphics resources to prevent memory leaks.
        /// Should be called when canvas is no longer needed.
        /// </summary>
        public void Dispose()
        {
            graphics?.Dispose();
            Pen?.Dispose();
            CanvasBitmap?.Dispose();
        }
    }
}