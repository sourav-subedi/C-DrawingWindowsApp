using BOOSE;
using MYBooseApp;
using System.Diagnostics;

namespace Medium_Scale_Software_Engineering_Project
{
    public partial class OutputForm : Form
    {
        ICanvas canvas;
        Graphics graphics;
        StoredProgram Program;
        CommandFactory Factory;
        IParser parser;

        public OutputForm()
        {
            InitializeComponent();
            Debug.WriteLine(AboutBOOSE.about());
            canvas = new AppCanvas(Height, Width);
            Factory = new AppCommandFactory();
            Program =new StoredProgram(canvas);
            parser = new Parser(Factory, Program);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Bitmap bitmap = (Bitmap)canvas.getBitmap();
            g.DrawImage(bitmap, 0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string code = multiLineInputBox.Text;

            try
            {
                Parser parser = new Parser();
                StoredProgram program = parser.ParseProgram(code);
                program.Execute();
                drawingBoard.Invalidate();
            }
            catch (Exception ex)
            {
                debugWindow.Text += ex.Message + Environment.NewLine;
            }
        }
    }
}
