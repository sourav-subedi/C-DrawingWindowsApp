namespace MYBooseApp
{
    partial class drawingApplication
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
            drawingBox = new PictureBox();
            singleCommandTextBox = new TextBox();
            runCommand = new Button();
            debugBox = new TextBox();
            debugLabel = new Label();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            commandListToolStripMenuItem = new ToolStripMenuItem();
            commandTextBox = new TextBox();
            clearDebug = new Button();
            runOneCommandButton = new Button();
            clearCanvas = new Button();
            fileToolStripMenuItem1 = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)drawingBox).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // drawingBox
            // 
            drawingBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            drawingBox.BackColor = SystemColors.ButtonHighlight;
            drawingBox.BorderStyle = BorderStyle.Fixed3D;
            drawingBox.Location = new Point(589, 85);
            drawingBox.Margin = new Padding(6);
            drawingBox.Name = "drawingBox";
            drawingBox.Size = new Size(1079, 850);
            drawingBox.TabIndex = 0;
            drawingBox.TabStop = false;
            drawingBox.Paint += drawingBox_Paint;
            // 
            // singleCommandTextBox
            // 
            singleCommandTextBox.Location = new Point(22, 896);
            singleCommandTextBox.Margin = new Padding(6);
            singleCommandTextBox.Name = "singleCommandTextBox";
            singleCommandTextBox.Size = new Size(524, 39);
            singleCommandTextBox.TabIndex = 2;
            // 
            // runCommand
            // 
            runCommand.ForeColor = SystemColors.ControlText;
            runCommand.Location = new Point(173, 958);
            runCommand.Margin = new Padding(6);
            runCommand.Name = "runCommand";
            runCommand.Size = new Size(139, 49);
            runCommand.TabIndex = 3;
            runCommand.Text = "Run";
            runCommand.UseVisualStyleBackColor = true;
            runCommand.Click += runCommand_Click;
            // 
            // debugBox
            // 
            debugBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            debugBox.Location = new Point(22, 1078);
            debugBox.Margin = new Padding(6);
            debugBox.Multiline = true;
            debugBox.Name = "debugBox";
            debugBox.ReadOnly = true;
            debugBox.ScrollBars = ScrollBars.Vertical;
            debugBox.Size = new Size(1646, 253);
            debugBox.TabIndex = 4;
            // 
            // debugLabel
            // 
            debugLabel.AutoSize = true;
            debugLabel.ForeColor = SystemColors.ActiveCaptionText;
            debugLabel.Location = new Point(22, 1030);
            debugLabel.Margin = new Padding(6, 0, 6, 0);
            debugLabel.Name = "debugLabel";
            debugLabel.Size = new Size(180, 32);
            debugLabel.TabIndex = 5;
            debugLabel.Text = "Debug Window";
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.White;
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(11, 4, 0, 4);
            menuStrip1.Size = new Size(1694, 46);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.BackColor = Color.White;
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, loadToolStripMenuItem, exitToolStripMenuItem, fileToolStripMenuItem1 });
            fileToolStripMenuItem.ForeColor = Color.Black;
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(71, 38);
            fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(359, 44);
            saveToolStripMenuItem.Text = "Save ";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(359, 44);
            loadToolStripMenuItem.Text = "Load";
            loadToolStripMenuItem.Click += loadToolStripMenuItem_Click;
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
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem, commandListToolStripMenuItem });
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
            // commandListToolStripMenuItem
            // 
            commandListToolStripMenuItem.Name = "commandListToolStripMenuItem";
            commandListToolStripMenuItem.Size = new Size(359, 44);
            commandListToolStripMenuItem.Text = "Command list";
            commandListToolStripMenuItem.Click += commandListToolStripMenuItem_Click;
            // 
            // commandTextBox
            // 
            commandTextBox.Location = new Point(22, 85);
            commandTextBox.Margin = new Padding(6);
            commandTextBox.Multiline = true;
            commandTextBox.Name = "commandTextBox";
            commandTextBox.Size = new Size(524, 793);
            commandTextBox.TabIndex = 9;
            commandTextBox.TextChanged += commandTextBox_TextChanged;
            commandTextBox.KeyPress += commandTextBox_KeyPress;
            // 
            // clearDebug
            // 
            clearDebug.Font = new Font("Segoe UI Black", 7.125F, FontStyle.Bold, GraphicsUnit.Point, 0);
            clearDebug.ForeColor = SystemColors.MenuText;
            clearDebug.Location = new Point(337, 958);
            clearDebug.Margin = new Padding(6);
            clearDebug.Name = "clearDebug";
            clearDebug.Size = new Size(116, 49);
            clearDebug.TabIndex = 10;
            clearDebug.Text = "Clear";
            clearDebug.UseVisualStyleBackColor = true;
            clearDebug.Click += clearDebug_Click;
            // 
            // runOneCommandButton
            // 
            runOneCommandButton.ForeColor = SystemColors.ActiveCaptionText;
            runOneCommandButton.Location = new Point(22, 958);
            runOneCommandButton.Margin = new Padding(6);
            runOneCommandButton.Name = "runOneCommandButton";
            runOneCommandButton.Size = new Size(139, 49);
            runOneCommandButton.TabIndex = 12;
            runOneCommandButton.Text = "Run One";
            runOneCommandButton.UseVisualStyleBackColor = true;
            runOneCommandButton.Click += runOneCommand_Click;
            // 
            // clearCanvas
            // 
            clearCanvas.BackColor = Color.IndianRed;
            clearCanvas.FlatStyle = FlatStyle.Flat;
            clearCanvas.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            clearCanvas.ForeColor = Color.Snow;
            clearCanvas.Location = new Point(1497, 972);
            clearCanvas.Margin = new Padding(0);
            clearCanvas.Name = "clearCanvas";
            clearCanvas.RightToLeft = RightToLeft.No;
            clearCanvas.Size = new Size(171, 49);
            clearCanvas.TabIndex = 13;
            clearCanvas.Text = "Clear Canvas";
            clearCanvas.UseVisualStyleBackColor = false;
            clearCanvas.Click += clearCanvas_Click;
            // 
            // fileToolStripMenuItem1
            // 
            fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            fileToolStripMenuItem1.Size = new Size(359, 44);
            fileToolStripMenuItem1.Text = "File";
            // 
            // drawingApplication
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(1694, 1361);
            Controls.Add(clearCanvas);
            Controls.Add(runOneCommandButton);
            Controls.Add(clearDebug);
            Controls.Add(commandTextBox);
            Controls.Add(debugLabel);
            Controls.Add(debugBox);
            Controls.Add(runCommand);
            Controls.Add(singleCommandTextBox);
            Controls.Add(drawingBox);
            Controls.Add(menuStrip1);
            ForeColor = SystemColors.Window;
            MainMenuStrip = menuStrip1;
            Margin = new Padding(6);
            Name = "drawingApplication";
            Text = "MyBooseApp";
            ((System.ComponentModel.ISupportInitialize)drawingBox).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox drawingBox;
        private TextBox singleCommandTextBox;
        private Button runCommand;
        private TextBox debugBox;
        private Label debugLabel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem commandListToolStripMenuItem;
        private TextBox commandTextBox;
        private Button clearDebug;
        private Button runOneCommandButton;
        private Button clearCanvas;
        private ToolStripMenuItem fileToolStripMenuItem1;
    }
}
