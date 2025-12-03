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

                debugWindow.AppendText($"[{DateTime.Now:HH:mm:ss}] Success!\r\n");
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?",
                                 "Exit",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            multiLineInputBox.Clear();
            canvas.Clear();
            drawingBoard.Image = canvas.getBitmap() as Bitmap;
            drawingBoard.Invalidate();
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "BOOSE File (*.txt)|*.txt|All Files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(dialog.FileName, multiLineInputBox.Text);
                MessageBox.Show("Code saved successfully!", "Success");
            }
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = canvas.getBitmap() as Bitmap;
                if (bmp != null)
                {
                    bmp.Save(dialog.FileName);
                    MessageBox.Show("Image saved successfully!", "Success");
                }
            }
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "BOOSE File (*.txt)|*.txt|All Files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                multiLineInputBox.Text = System.IO.File.ReadAllText(dialog.FileName);
                MessageBox.Show("Code loaded successfully!", "Success");
            }
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.png;*.jpg)|*.png;*.jpg";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(dialog.FileName);

                drawingBoard.Image = bmp;
                drawingBoard.Invalidate();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("BOOSE Editor\nBuilt by Sourav\n2025\nUsing BOOSE Interpreter",
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}