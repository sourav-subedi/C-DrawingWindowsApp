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
            multiLineInputBox = new TextBox();
            debugWindow = new TextBox();
            runButton = new Button();
            drawingBoard = new PictureBox();
            singleLineInputBox = new TextBox();
            commandLabel = new Label();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            saveFileToolStripMenuItem = new ToolStripMenuItem();
            saveImageToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            loadFileToolStripMenuItem = new ToolStripMenuItem();
            loadImageToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)drawingBoard).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // multiLineInputBox
            // 
            multiLineInputBox.BackColor = Color.White;
            multiLineInputBox.Location = new Point(26, 58);
            multiLineInputBox.Multiline = true;
            multiLineInputBox.Name = "multiLineInputBox";
            multiLineInputBox.ScrollBars = ScrollBars.Vertical;
            multiLineInputBox.Size = new Size(683, 458);
            multiLineInputBox.TabIndex = 0;
            // 
            // debugWindow
            // 
            debugWindow.Location = new Point(36, 634);
            debugWindow.Multiline = true;
            debugWindow.Name = "debugWindow";
            debugWindow.ReadOnly = true;
            debugWindow.ScrollBars = ScrollBars.Vertical;
            debugWindow.Size = new Size(673, 262);
            debugWindow.TabIndex = 1;
            // 
            // runButton
            // 
            runButton.BackColor = SystemColors.GradientActiveCaption;
            runButton.Location = new Point(36, 582);
            runButton.Name = "runButton";
            runButton.Size = new Size(107, 46);
            runButton.TabIndex = 2;
            runButton.Text = "Run";
            runButton.UseVisualStyleBackColor = false;
            runButton.Click += button1_Click;
            // 
            // drawingBoard
            // 
            drawingBoard.BackColor = SystemColors.ButtonShadow;
            drawingBoard.BorderStyle = BorderStyle.FixedSingle;
            drawingBoard.Location = new Point(715, 58);
            drawingBoard.Name = "drawingBoard";
            drawingBoard.Size = new Size(1254, 838);
            drawingBoard.TabIndex = 3;
            drawingBoard.TabStop = false;
            drawingBoard.Paint += pictureBox1_Paint;
            // 
            // singleLineInputBox
            // 
            singleLineInputBox.Location = new Point(176, 542);
            singleLineInputBox.Name = "singleLineInputBox";
            singleLineInputBox.Size = new Size(495, 39);
            singleLineInputBox.TabIndex = 4;
            // 
            // commandLabel
            // 
            commandLabel.AutoSize = true;
            commandLabel.Location = new Point(35, 542);
            commandLabel.Name = "commandLabel";
            commandLabel.Size = new Size(130, 32);
            commandLabel.TabIndex = 5;
            commandLabel.Text = "Command:";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1988, 42);
            menuStrip1.TabIndex = 6;
            menuStrip1.Text = "topMenuBar";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, saveAsToolStripMenuItem, loadToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(71, 38);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(359, 44);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveFileToolStripMenuItem, saveImageToolStripMenuItem });
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(359, 44);
            saveAsToolStripMenuItem.Text = "Save As";
            // 
            // saveFileToolStripMenuItem
            // 
            saveFileToolStripMenuItem.Name = "saveFileToolStripMenuItem";
            saveFileToolStripMenuItem.Size = new Size(359, 44);
            saveFileToolStripMenuItem.Text = "Save File";
            saveFileToolStripMenuItem.Click += saveFileToolStripMenuItem_Click;
            // 
            // saveImageToolStripMenuItem
            // 
            saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            saveImageToolStripMenuItem.Size = new Size(359, 44);
            saveImageToolStripMenuItem.Text = "Save Image";
            saveImageToolStripMenuItem.Click += saveImageToolStripMenuItem_Click;
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadFileToolStripMenuItem, loadImageToolStripMenuItem });
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(359, 44);
            loadToolStripMenuItem.Text = "Load";
            // 
            // loadFileToolStripMenuItem
            // 
            loadFileToolStripMenuItem.Name = "loadFileToolStripMenuItem";
            loadFileToolStripMenuItem.Size = new Size(359, 44);
            loadFileToolStripMenuItem.Text = "Load File";
            loadFileToolStripMenuItem.Click += loadFileToolStripMenuItem_Click;
            // 
            // loadImageToolStripMenuItem
            // 
            loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
            loadImageToolStripMenuItem.Size = new Size(359, 44);
            loadImageToolStripMenuItem.Text = "Load Image";
            loadImageToolStripMenuItem.Click += loadImageToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(359, 44);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(84, 38);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(359, 44);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // OutputForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1988, 908);
            Controls.Add(commandLabel);
            Controls.Add(singleLineInputBox);
            Controls.Add(drawingBoard);
            Controls.Add(runButton);
            Controls.Add(debugWindow);
            Controls.Add(multiLineInputBox);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "OutputForm";
            Text = "MyBoose App";
            ((System.ComponentModel.ISupportInitialize)drawingBoard).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox multiLineInputBox;
        private TextBox debugWindow;
        private Button runButton;
        private PictureBox drawingBoard;
        private TextBox singleLineInputBox;
        private Label commandLabel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem saveFileToolStripMenuItem;
        private ToolStripMenuItem saveImageToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem loadFileToolStripMenuItem;
        private ToolStripMenuItem loadImageToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}
