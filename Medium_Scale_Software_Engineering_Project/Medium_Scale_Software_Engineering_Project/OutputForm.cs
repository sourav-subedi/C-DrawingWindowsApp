using System;
using System.Drawing;
using System.Windows.Forms;
using BOOSE;
using MYBooseApp;

namespace Medium_Scale_Software_Engineering_Project
{
    public partial class OutputForm : Form
    {
        private AppCanvas canvas;
        private AppCommandFactory factory;
        private StoredProgram program;
        private AppParser parser;



        public OutputForm()
        {
            InitializeComponent();

            this.Load += (s, e) =>
            {
                canvas = new AppCanvas(drawingBoard.Width, drawingBoard.Height);
                factory = new AppCommandFactory();  // Use DLL factory
                program = new StoredProgram(canvas);
                parser = new AppParser(factory, program);

                
                canvas.Clear();
                canvas.PenColour = Color.Black;
                canvas.WriteText("BOOSE Ready", 20, 50);
                RefreshCanvas();

            };
        }

        private void RefreshCanvas()
        {
            drawingBoard.Image = canvas.getBitmap() as Bitmap;
            drawingBoard.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string code = multiLineInputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(code)) return;

            canvas.Clear();
            canvas.PenColour = Color.Black;
            RefreshCanvas();
            debugWindow.Clear();

            try
            {
                parser.Parse(code);  
                program.Run();         

                debugWindow.AppendText($"[{DateTime.Now:HH:mm:ss}] Success! 5 circles drawn.\r\n");
            }
            catch (ParserException pe)
            {
                debugWindow.AppendText($"[{DateTime.Now:HH:mm:ss}] SYNTAX ERROR: {pe.Message}\r\n");
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"[{DateTime.Now:HH:mm:ss}] ERROR: {ex.Message}\r\n");
            }
            finally
            {
                RefreshCanvas();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (canvas?.getBitmap() is Bitmap bmp)
                e.Graphics.DrawImage(bmp, 0, 0);
        }
    }
}