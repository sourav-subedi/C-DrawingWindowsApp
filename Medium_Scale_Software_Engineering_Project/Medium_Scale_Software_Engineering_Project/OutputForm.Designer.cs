namespace Medium_Scale_Software_Engineering_Project
{
    partial class OutputForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            debugWindow = new TextBox();
            button1 = new Button();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.White;
            textBox1.Location = new Point(36, 41);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(673, 475);
            textBox1.TabIndex = 0;
            // 
            // debugWindow
            // 
            debugWindow.Location = new Point(36, 641);
            debugWindow.Multiline = true;
            debugWindow.Name = "debugWindow";
            debugWindow.ReadOnly = true;
            debugWindow.ScrollBars = ScrollBars.Vertical;
            debugWindow.Size = new Size(673, 255);
            debugWindow.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(36, 549);
            button1.Name = "button1";
            button1.Size = new Size(150, 46);
            button1.TabIndex = 2;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(715, 41);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1223, 855);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            // 
            // OutputForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1988, 908);
            Controls.Add(pictureBox1);
            Controls.Add(button1);
            Controls.Add(debugWindow);
            Controls.Add(textBox1);
            Name = "OutputForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox debugWindow;
        private Button button1;
        private PictureBox pictureBox1;
    }
}
