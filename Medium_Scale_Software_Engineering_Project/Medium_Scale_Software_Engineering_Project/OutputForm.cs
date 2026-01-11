using BOOSE;
using System.Data;
using System.Diagnostics;


namespace MYBooseApp
{
    /// <summary>
    /// WinForms drawing application form class with canvas, command factory, stored program and parser
    /// </summary>
    public partial class drawingApplication : Form
    {
        AppCanvas canvas; //appcanvas reference
        AppCommandFactory Factory; //command factory reference
        AppStoredProgram Program; //stored program reference
        Parser Parser; //BOOSE parser reference
        private readonly string[] allCommands = new string[]
        {
            "circle radius [true/false]",
            "rect width height [true/false]",
            "tri width height",
            "moveto x y",
            "drawto x y",
            "pen color",
            "pensize size",
            "write text",
            "clear",
            "reset",
            "int varname = value",
            "real varname = value",
            "boolean varname = value",
            "array arrayname = size",
            "poke arrayname index value",
            "peek arrayname index",
            "if condition",
            "else",
            "end if",
            "while condition",
            "end while",
            "for var = start to end [step increment]",
            "end for",
            "method [returnType] methodName [params]",
            "end method",
            "call methodName [args]"
        };

        /// <summary>
        /// Constructor for drawingApplication form. Initializes canvas and sets canvas refernce to commandfactory, program
        /// makes parser work with command factory and stored program
        /// </summary>
        public drawingApplication()
        {
            InitializeComponent();
            Debug.WriteLine(AboutBOOSE.about());
            canvas = AppCanvas.Instance(1280, 1080);
            Factory = new AppCommandFactory(canvas); // pass canvas here
            Program = new AppStoredProgram(canvas);
            Parser = new Parser(Factory, Program);
            debugBox.AppendText(AboutBOOSE.about() + Environment.NewLine);

            // Link the off-screen bitmap to the PictureBox so that changes are visible immediately.
            // The PictureBox will now display the same Bitmap that AppCanvas draws on.
            drawingBox.Image = (Bitmap)canvas.getBitmap();

            // Ensure the image fits the PictureBox properly without distortion
            drawingBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        /// <summary>
        /// Paint event of drawing panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawingBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Bitmap b = (Bitmap)canvas.getBitmap();
            g.DrawImage(b, 0, 0);
        }

        /// <summary>
        /// Executes all commands in the main textbox first, then the single command textbox if it has content.
        /// Each command is parsed and added to the stored program, then executed.
        /// </summary>
        private void ExecuteCommands()
        {
            debugBox.Clear();

            // Clear and reset before starting
            Program.Clear();
            Program.ResetProgram();

            try
            {
                // STEP 1: Parse all commands from main textbox
                string mainCode = commandTextBox.Text;
                if (!string.IsNullOrWhiteSpace(mainCode))
                {
                    // Split lines and remove empty ones immediately
                    string[] lines = mainCode
                        .Replace("\r", "")
                        .Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(l => !string.IsNullOrWhiteSpace(l.Trim()))
                        .ToArray();

                    int lineNumber = 1;
                    foreach (string line in lines)
                    {
                        string trimmedLine = line.Trim();
                        if (string.IsNullOrWhiteSpace(trimmedLine))
                            continue;

                        string lineToParse = trimmedLine;

                        // Normalize end-if variants to "end"
                        string lower = trimmedLine.ToLowerInvariant();
                        if (lower.StartsWith("end if") || lower == "endif" || lower == "end")
                        {
                            lineToParse = "end";
                        }
                        else if (lower == "endwhile" || lower.StartsWith("end while"))
                        {
                            // Normalize while endings → EndAppWhile
                            lineToParse = "endwhile";
                        }
                        else if (lower == "endfor" || lower.StartsWith("end for"))
                        {
                            // Normalize for endings → EndForCommand
                            lineToParse = "endfor";
                        }
                        else if (lower == "endmethod" || lower.StartsWith("end method"))
                        {
                            // Normalize method endings → EndMethodCommand
                            lineToParse = "endmethod";
                        }
                        else if (IsAssignmentLine(trimmedLine))
                        {
                            lineToParse = NormalizeAssignment(trimmedLine);
                        }

                        try
                        {
                            Debug.WriteLine($"Line {lineNumber}: Parsing '{lineToParse}' (original: '{trimmedLine}')");

                            int countBefore = Program.Count;  // Check count BEFORE parsing

                            ICommand command = Parser.ParseCommand(lineToParse);

                            int countAfter = Program.Count;  // Check count AFTER parsing


                            // Only add if parser didn't already add it
                            if (countAfter == countBefore)
                            {
                                Program.Add(command);
                                Debug.WriteLine($"Manually added command at index {Program.Count - 1}");
                            }
                            else
                            {
                                Debug.WriteLine($"Parser auto-added command (count increased from {countBefore} to {countAfter})");
                            }

                            Debug.WriteLine($"Line {lineNumber}: Added {command.GetType().Name} at index {Program.Count - 1}");
                            debugBox.AppendText($"Line {lineNumber}: Parsed: {trimmedLine}\r\n");
                        }
                        catch (Exception ex)
                        {
                            debugBox.AppendText($"Line {lineNumber}: Parse error '{trimmedLine}': {ex.Message}\r\n");
                        }

                        lineNumber++;
                    }
                }

                // STEP 2: Single command (same cleaning)
                string singleCode = singleCommandTextBox.Text;
                if (!string.IsNullOrWhiteSpace(singleCode))
                {
                    try
                    {
                        string singleToParse = singleCode.Trim();
                        string lowerSingle = singleToParse.ToLowerInvariant();
                        if (lowerSingle.StartsWith("end if") || lowerSingle == "endif" || lowerSingle == "end")
                            singleToParse = "end";
                        else if (IsAssignmentLine(singleToParse))
                            singleToParse = NormalizeAssignment(singleToParse);

                        ICommand singleCmd = Parser.ParseCommand(singleToParse);
                        Program.Add(singleCmd);
                        debugBox.AppendText($"Single command parsed: {singleCode}\r\n");
                    }
                    catch (Exception ex)
                    {
                        debugBox.AppendText($"Single command parse error: {ex.Message}\r\n");
                    }
                }

                // STEP 3: Run and refresh
                if (Program.Count > 0)
                {
                    debugBox.AppendText("\r\n--- Executing Program ---\r\n");
                    Program.Run();
                    debugBox.AppendText("✓ Program executed successfully!\r\n");

                    // Force canvas refresh
                    drawingBox.Image = (Bitmap)canvas.getBitmap();
                    drawingBox.Invalidate();
                    drawingBox.Update();
                }
            }
            catch (Exception ex)
            {
                debugBox.AppendText($"Execution error: {ex.Message}\r\n");
            }
        }
        /// <summary>
        /// Executes single command from the single command textbox
        /// </summary>
        /// <param name="commandLine">the command written on the text box</param>
        private void ExecuteSingleCommand(string commandLine)
        {
            debugBox.Clear();
            if (string.IsNullOrWhiteSpace(commandLine))
            {
                debugBox.AppendText("No command entered.\r\n");
                return;
            }
            try
            {
                // Parse the command
                string lineToParse = commandLine.Trim();

                if (IsAssignmentLine(lineToParse))
                {
                    lineToParse = NormalizeAssignment(lineToParse);
                }

                ICommand command = Parser.ParseCommand(lineToParse);
                // Add to program and execute
                Program.Add(command);
                Program.Run();
                debugBox.AppendText("Single command executed successfully.\r\n");

                // Force repaint after execution
                drawingBox.Invalidate();
            }
            catch (Exception ex)
            {
                debugBox.AppendText($"Error: {ex.Message}\r\n");
            }
        }

        /// <summary>
        /// run button for textbox which calls the execute command method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runCommand_Click(object sender, EventArgs e)
        {
            ExecuteCommands();
        }

        /// <summary>
        /// Event handler for key press in command text box shift + enter to execute commands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commandTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Shift + Enter to execute commands
            if (e.KeyChar == (char)Keys.Enter && Control.ModifierKeys == Keys.Shift)
            {
                e.Handled = true; // prevent newline
                ExecuteCommands();
            }
        }

        /// <summary>
        /// clears the debug text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearDebug_Click(object sender, EventArgs e)
        {
            // Clear the debug box
            debugBox.Text = "";
        }

        /// <summary>
        /// button command for single textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runOneCommand_Click(object sender, EventArgs e)
        {
            ExecuteSingleCommand(singleCommandTextBox.Text);
        }

        /// <summary>
        /// Displays about BOOSE information in the debug box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            debugBox.AppendText(AboutBOOSE.about() + Environment.NewLine);
        }

        /// <summary>
        /// Opens a new form displaying the list of available commands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commandListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a new form
                Form commandListForm = new Form
                {
                    Text = "Available Commands",
                    Width = 400,
                    Height = 500,
                    StartPosition = FormStartPosition.CenterParent
                };
                // Add a ListBox to display commands
                ListBox listBox = new ListBox
                {
                    Dock = DockStyle.Fill,
                    Font = new System.Drawing.Font("Segoe UI", 10)
                };
                listBox.Items.AddRange(allCommands);
                // Add the ListBox to the form
                commandListForm.Controls.Add(listBox);
                // Show the form
                commandListForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open command list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Exits the application when the Exit menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close the main form and exit the application
            Application.Exit();
        }

        /// <summary>
        /// Opens a save file dialog to save the current canvas as PNG and commands as BOOSE file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Title = "Save BOOSE Drawing";
                    sfd.Filter = "PNG Image (*.png)|*.png|BOOSE File (*.boose)|*.boose";
                    sfd.AddExtension = true;
                    sfd.DefaultExt = "boose";
                    sfd.InitialDirectory = Path.Combine(Application.StartupPath, "saves");

                    // Ensure saves folder exists
                    if (!Directory.Exists(sfd.InitialDirectory))
                        Directory.CreateDirectory(sfd.InitialDirectory);

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = sfd.FileName;
                        string ext = Path.GetExtension(filePath).ToLower();

                        // Save PNG
                        if (ext == ".png" || ext == "")
                        {
                            var bmpObj = canvas.getBitmap();
                            if (bmpObj is Bitmap bmp)
                                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                            else if (bmpObj is Image img)
                                new Bitmap(img).Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                            else
                                MessageBox.Show("Canvas bitmap is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // Save BOOSE commands
                        if (ext == ".boose" || ext == "")
                        {
                            File.WriteAllText(Path.ChangeExtension(filePath, ".boose"), commandTextBox.Text);
                        }

                        MessageBox.Show($"Saved successfully!\nFile: {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Saves the current canvas png and commands to a BOOSE file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Go one level above the solution folder
                string parentFolder = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\.."));
                string savesFolder = Path.Combine(parentFolder, "saves");

                if (!Directory.Exists(savesFolder))
                {
                    MessageBox.Show("Saves folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Open file dialog to select a .boose file
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.InitialDirectory = savesFolder;
                    ofd.Filter = "BOOSE Files (*.boose)|*.boose";
                    ofd.Title = "Load BOOSE File";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        // Read the commands from the file
                        string commands = File.ReadAllText(ofd.FileName);
                        // Put the commands into the main command textbox
                        commandTextBox.Text = commands;
                        // Optionally clear single command box
                        singleCommandTextBox.Clear();
                        // Optionally execute loaded commands immediately
                        ExecuteCommands();
                        debugBox.AppendText($"Loaded commands from {Path.GetFileName(ofd.FileName)}\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// displays similar commands as user types in the command textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commandTextBox_TextChanged(object sender, EventArgs e)
        {
            string input = commandTextBox.Text.Trim().ToLower();

            // Clear suggestions first
            

            if (string.IsNullOrWhiteSpace(input))
                return; // No input, no suggestions

            // Find matching commands
            var matches = allCommands
                .Where(cmd => cmd.ToLower().Contains(input))
                .ToList();

            
        }

        /// <summary>
        /// Resets the canvas and stored program when clear canvas button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearCanvas_Click(object sender, EventArgs e)
        {
            canvas.Clear(); 
            drawingBox.Invalidate();
            Program.ResetProgram();
        }

        /// <summary>
        /// detects direct assignment
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool IsAssignmentLine(string line)
        {
            string trimmed = line.TrimStart();
            if (!trimmed.Contains("=")) return false;

            string firstWord = trimmed.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0].ToLower();

            // Skip declaration keywords
            if (firstWord == "int" || firstWord == "real" || firstWord == "array" || firstWord == "boolean")
                return false;

            // Skip known commands (including end, set)
            string[] knownCommands = { "circle", "moveto", "drawto", "pen", "rect", "pensize", "tri", "write", "clear", "reset", "poke", "peek", "set", "if", "else", "end", "while", "endwhile", "for", "endfor", "method", "endmethod" };
            if (System.Array.IndexOf(knownCommands, firstWord) >= 0)
                return false;

            return true;
        }


        private string NormalizeAssignment(string line)
        {
            string trimmed = line.Trim();

            // If it already starts with "set", return as-is (prevent double-prefix)
            if (trimmed.ToLower().StartsWith("set "))
                return trimmed;

            // Add "set" prefix for BOOSE AppAsign
            return "set " + trimmed;
        }


    }
}