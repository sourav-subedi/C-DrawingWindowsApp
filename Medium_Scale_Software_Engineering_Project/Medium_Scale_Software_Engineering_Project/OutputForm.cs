using BOOSE;
//using BOOSEDrawingApp;
using MYBooseApp;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Medium_Scale_Software_Engineering_Project
{
    /// <summary>
    /// Represents the main output and editor form for the BOOSE graphics application.
    /// Handles program input, parsing, execution, canvas drawing, file operations, and UI commands.
    /// </summary>
    public partial class OutputForm : Form
    {
        /// <summary>
        /// The application canvas used for drawing shapes and text.
        /// </summary>
        private AppCanvas canvas;

        /// <summary>
        /// Command factory responsible for creating BOOSE command objects.
        /// </summary>
        private AppCommandFactory factory;

        /// <summary>
        /// Stores the parsed BOOSE program before execution.
        /// </summary>
        private AppStoredProgram program;

        /// <summary>
        /// The parser responsible for converting text commands into BOOSE executable commands.
        /// </summary>
        private AppParser parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputForm"/> class.
        /// Sets up the BOOSE environment, canvas, parser, and UI.
        /// </summary>
        public OutputForm()
        {
            InitializeComponent();
            Debug.WriteLine(AboutBOOSE.about());

            this.Load += (s, e) =>
            {
                try
                {
                    canvas = new AppCanvas(drawingBoard.Width, drawingBoard.Height);
                    factory = new AppCommandFactory();
                    program = new AppStoredProgram(canvas);
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

        /// <summary>
        /// Refreshes the drawing canvas displayed on the PictureBox.
        /// </summary>
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

        /// <summary>
        /// Executes when the "Run" button is clicked.
        /// Parses user input line-by-line, handles multi-line statements,
        /// and runs the BOOSE program when all lines are valid.
        /// </summary>
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
                debugWindow.AppendText("=== Starting Execution ===\r\n");

                // Reset the program to clear old commands
                program.ResetProgram();
                debugWindow.AppendText("Program reset.\r\n");

                // Parse the entire program at once (BOOSE handles line breaks)
                debugWindow.AppendText($"Parsing code...\r\n");
                parser.ParseProgram(code);

                debugWindow.AppendText($"Parsed {program.Count} commands.\r\n");

                if (program.Count == 0)
                {
                    debugWindow.AppendText("WARNING: No commands were parsed!\r\n");
                    return;
                }

                // Run the program
                debugWindow.AppendText("Running program...\r\n");
                program.Run();

                debugWindow.AppendText($"[{DateTime.Now:HH:mm:ss}] Success!\r\n");
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"[{DateTime.Now:HH:mm:ss}] ERROR: {ex.Message}\r\n");
                if (ex.InnerException != null)
                {
                    debugWindow.AppendText($"Inner: {ex.InnerException.Message}\r\n");
                }
            }
            finally
            {
                debugWindow.AppendText("Refreshing canvas...\r\n");
                RefreshCanvas();
            }
        }

        /// <summary>
        /// Handles custom drawing of the PictureBox to ensure the canvas bitmap is displayed correctly.
        /// </summary>
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

        /// <summary>
        /// Confirms before closing the application when the Exit menu item is clicked.
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "Are you sure you want to exit?",
                    "Exit",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                    Application.Exit();
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"EXIT ERROR: {ex.Message}\r\n");
            }
        }

        /// <summary>
        /// Clears input and resets the canvas when creating a new program.
        /// </summary>
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

        /// <summary>
        /// Saves the contents of the editor to a text file.
        /// </summary>
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

        /// <summary>
        /// Saves the current canvas image to disk.
        /// </summary>
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

        /// <summary>
        /// Loads a BOOSE program text file into the editor.
        /// </summary>
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

        /// <summary>
        /// Loads an image file into the drawing board.
        /// </summary>
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

        /// <summary>
        /// Displays application information.
        /// </summary>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(
                    "BOOSE Editor\nBuilt by Sourav\n2025\nUsing BOOSE Interpreter",
                    "About",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                debugWindow.AppendText($"ABOUT ERROR: {ex.Message}\r\n");
            }
        }

        /// <summary>
        /// Handles Enter key input for the single-line command box.
        /// </summary>
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

        /// <summary>
        /// Executes quick UI commands (new, save, load, exit) typed in the single-line command box.
        /// </summary>
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
