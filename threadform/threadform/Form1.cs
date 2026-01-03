using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace threadform
{
    public partial class Form1 : Form
    {
        Thread newThread;
        bool flag = false, running = false;
        public Form1()
        {
            InitializeComponent();
            newThread = new System.Threading.Thread(thread);
            newThread.Start();

        }

        public void thread()
        {
            while (true)
            {
                while (running == true)
                {
                    if (flag == false)
                    {
                        this.button1.BackColor = System.Drawing.Color.Red;
                        flag = true;

                    }
                    else
                    {
                        this.button1.BackColor = System.Drawing.SystemColors.ActiveBorder;
                        flag = false;
                    }
                    Thread.Sleep(1000);
                }
            }
 
        }
        private void button1_Click(object sender, EventArgs e)
        {
            running = !running;
            //newThread.Abort();
        }
    }
}
