using BOOSE;
using System.Diagnostics;

namespace Medium_Scale_Software_Engineering_Project
{
    public partial class OutputForm : Form
    {
        AppCanvas canvas;
        public OutputForm()
        {
            InitializeComponent();
            Debug.WriteLine(AboutBOOSE.about());
            canvas = new AppCanvas(Height, Width);
            Parser.staticCanavas = canvas;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Bitmap bitmap = (Bitmap)canvas.getBitmap();
            g.DrawImage(bitmap, 0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string code = textBox1.Text;

            try
            {
                Parser parser = new Parser();
                StoredProgram program = parser.ParseProgram(code);
                program.Execute();
                pictureBox1.Invalidate();
            }
            catch (Exception ex)
            {
                debugWindow.Text += ex.Message + Environment.NewLine;
            }
        }
    }
}
