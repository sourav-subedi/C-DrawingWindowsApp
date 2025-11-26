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
            canvas= new AppCanvas(Height,Width);
            canvas.Circle(300,true);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Bitmap bitmap = (Bitmap) canvas.getBitmap();
            g.DrawImage(bitmap,0,0);
        }
    }
}
