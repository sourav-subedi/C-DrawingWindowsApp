using BOOSE;
using MYBooseApp;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
                try
                {
                    canvas = new AppCanvas(drawingBoard.Width, drawingBoard.Height);
                    factory = new AppCommandFactory();
                    program = new StoredProgram(canvas);
                    parser = new AppParser(factory, program);

                    canvas.Clear();
                    canvas.PenColour = Color.Black;
                    canvas.WriteText("BOOSE Ready", 20, 50);
                    RefreshCanvas();
                }
                catch (Exception ex)
                {
                    debugWindow.AppendText($"INIT ERROR: {ex.Message}\r\n");
                }
            };
        }

        private void RefreshCanvas()
        {
            try
            {
                drawingBoard.Image = canvas?.getBitmap() as Bitmap;
                drawingBoard.Invalidate();
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"REFRESH ERROR: {ex.Message}\r\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string code = multiLineInputBox.Text;
            if (string.IsNullOrWhiteSpace(code))
            {
                debugWindow.AppendText("No code entered.\r\n");
                return;
            }

            canvas.Clear();
            canvas.PenColour = Color.Black;
            RefreshCanvas();
            debugWindow.Clear();

            try
            {
                string[] lines = code.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                StringBuilder statementBuffer = new StringBuilder();
                int statementStartLine = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    if (statementBuffer.Length == 0)
                        statementStartLine = i + 1; // remember start line of this statement

                    statementBuffer.AppendLine(line);

                    // Check if statement ends (example: ";" or custom logic for blocks)
                    if (line.EndsWith(";") || line == "end") // adjust according to your language
                    {
                        try
                        {
                            parser.Parse(statementBuffer.ToString()); // parse full statement
                        }
                        catch (Exception ex)
                        {
                            debugWindow.AppendText(
                                $"[{DateTime.Now:HH:mm:ss}] ERROR near line {statementStartLine}: {ex.Message}\r\n"
                            );
                            return; // stop execution on error
                        }

                        statementBuffer.Clear(); // reset buffer for next statement
                    }
                }

                // Parse any leftover statement
                if (statementBuffer.Length > 0)
                {
                    try
                    {
                        parser.Parse(statementBuffer.ToString());
                    }
                    catch (Exception ex)
                    {
                        debugWindow.AppendText(
                            $"[{DateTime.Now:HH:mm:ss}] ERROR near line {statementStartLine}: {ex.Message}\r\n"
                        );
                        return;
                    }
                }

                program.Run();
                debugWindow.AppendText($"[{DateTime.Now:HH:mm:ss}] Success!\r\n");
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
            try
            {
                if (canvas?.getBitmap() is Bitmap bmp)
                    e.Graphics.DrawImage(bmp, 0, 0);
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"PAINT ERROR: {ex.Message}\r\n");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                debugWindow.AppendText($"EXIT ERROR: {ex.Message}\r\n");
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                multiLineInputBox.Clear();
                canvas.Clear();
                RefreshCanvas();
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"RESET ERROR: {ex.Message}\r\n");
            }
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "BOOSE File (*.txt)|*.txt|All Files (*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(dialog.FileName, multiLineInputBox.Text);
                    MessageBox.Show("Code saved successfully!", "Success");
                }
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"SAVE FILE ERROR: {ex.Message}\r\n");
            }
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (canvas?.getBitmap() is Bitmap bmp)
                    {
                        bmp.Save(dialog.FileName);
                        MessageBox.Show("Image saved successfully!", "Success");
                    }
                    else
                    {
                        debugWindow.AppendText("No image available to save.\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"SAVE IMAGE ERROR: {ex.Message}\r\n");
            }
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "BOOSE File (*.txt)|*.txt|All Files (*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    multiLineInputBox.Text = System.IO.File.ReadAllText(dialog.FileName);
                    MessageBox.Show("Code loaded successfully!", "Success");
                }
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"LOAD FILE ERROR: {ex.Message}\r\n");
            }
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                debugWindow.AppendText($"LOAD IMAGE ERROR: {ex.Message}\r\n");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("BOOSE Editor\nBuilt by Sourav\n2025\nUsing BOOSE Interpreter",
                    "About",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"ABOUT ERROR: {ex.Message}\r\n");
            }
        }

        private void singleLineInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    HandleSingleCommand(singleLineInputBox.Text.Trim());
                    singleLineInputBox.Clear();
                }
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"COMMAND INPUT ERROR: {ex.Message}\r\n");
            }
        }

        private void HandleSingleCommand(string command)
        {
            try
            {
                command = command.ToLower().Trim();

                switch (command)
                {
                    case "new":
                    case "reset":
                    case "clear":
                        newToolStripMenuItem_Click(null, null);
                        break;

                    case "save":
                        saveFileToolStripMenuItem_Click(null, null);
                        break;

                    case "saveimage":
                        saveImageToolStripMenuItem_Click(null, null);
                        break;

                    case "load":
                    case "open":
                        loadFileToolStripMenuItem_Click(null, null);
                        break;

                    case "loadimage":
                        loadImageToolStripMenuItem_Click(null, null);
                        break;

                    case "exit":
                    case "quit":
                        Application.Exit();
                        break;

                    default:
                        MessageBox.Show("Unknown command: " + command);
                        break;
                }
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"SINGLE COMMAND ERROR: {ex.Message}\r\n");
            }
        }
    }
}
