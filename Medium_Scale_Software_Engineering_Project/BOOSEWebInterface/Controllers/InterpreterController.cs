using MYBooseApp;
using BOOSE;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Input;

namespace BOOSEWebAPI.Controllers
{
    /// <summary>
    /// API Controller for executing BOOSE interpreter commands and returning canvas output
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InterpreterController : ControllerBase
    {
        /// <summary>
        /// Executes BOOSE commands and returns the canvas as a Base64 image
        /// </summary>
        /// <param name="request">Request containing the BOOSE commands to execute</param>
        /// <returns>JSON response with success status, image, and debug information</returns>
        [HttpPost("execute")]
        public IActionResult ExecuteCommands([FromBody] ExecuteRequest request)
        {
            try
            {
                // Create canvas and program (same as Form1.cs constructor) 
                var canvas = new WebCanvas(1280, 1080);
                var factory = new AppCommandFactory(canvas); 
                var program = new AppStoredProgram(canvas); 
                var parser = new Parser(factory, program);

                // Split commands into lines 
                string[] lines = request.Commands
                    .Replace("\r", "")
                    .Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(l => !string.IsNullOrWhiteSpace(l.Trim()))
                    .ToArray();

                int lineNumber = 1;
                List<string> debugOutput = new List<string>();

                // Parse each command
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (string.IsNullOrWhiteSpace(trimmedLine))
                        continue;

                    string lineToParse = NormalizeCommand(trimmedLine);

                    try
                    {
                        int countBefore = program.Count;  

                        BOOSE.ICommand command = parser.ParseCommand(lineToParse);

                        int countAfter = program.Count;   

                        if (countAfter == countBefore)
                        {
                            program.Add(command);
                            debugOutput.Add($"Line {lineNumber}: Parsed '{trimmedLine}'");
                        }
                        else
                        {
                            debugOutput.Add($"Line {lineNumber}: Parsed '{trimmedLine}' (auto-added by parser)");
                        }
                    }
                    catch (Exception ex)
                    {
                        debugOutput.Add($"Line {lineNumber}: Error - {ex.Message}");
                    }

                    lineNumber++;
                }

                debugOutput.Add($"Total commands parsed: {program.Count}");

                // Check if methods are properly linked
                for (int i = 0; i < program.Count; i++)
                {
                    var cmd = ((AppStoredProgram)program).GetCommand(i);
                    if (cmd is AppMethod methodCmd)
                    {
                        debugOutput.Add($"Method at {i}: endIndex = {methodCmd.GetMethodStartIndex()}");
                    }
                }

                // Execute the program [6]
                program.Run();
                debugOutput.Add("Program executed successfully!");

                // Get the canvas as Base64 image
                string base64Image = canvas.GetBase64Image();

                return Ok(new
                {
                    success = true,
                    image = base64Image,
                    debug = string.Join("\n", debugOutput)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Normalizes command syntax (same logic as Form1.cs) [4]
        /// </summary>
        private string NormalizeCommand(string line)
        {
            string lower = line.ToLowerInvariant();

            // Normalize end statements [4]
            if (lower.StartsWith("end if") || lower == "endif" || lower == "end")
                return "end";
            if (lower == "endwhile" || lower.StartsWith("end while"))
                return "endwhile";
            if (lower == "endfor" || lower.StartsWith("end for"))
                return "endfor";
            if (lower == "endmethod" || lower.StartsWith("end method"))
                return "endmethod";

            // Normalize assignments [4]
            if (IsAssignmentLine(line))
                return NormalizeAssignment(line);

            return line;
        }

        /// <summary>
        /// Detects if a line is a direct assignment [4]
        /// </summary>
        private bool IsAssignmentLine(string line)
        {
            string trimmed = line.TrimStart();
            if (!trimmed.Contains("=")) return false;

            string firstWord = trimmed.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0].ToLower();

            // Skip declaration keywords
            if (firstWord == "int" || firstWord == "real" || firstWord == "array" || firstWord == "boolean")
                return false;

            // Skip known commands [5]
            string[] knownCommands = { "circle", "moveto", "drawto", "pen", "rect", "pensize", "tri", "write",
                                      "clear", "reset", "poke", "peek", "set", "if", "else", "end",
                                      "while", "endwhile", "for", "endfor", "method", "endmethod", "call" };
            if (System.Array.IndexOf(knownCommands, firstWord) >= 0)
                return false;

            return true;
        }

        /// <summary>
        /// Adds "set" prefix to assignment statements [4]
        /// </summary>
        private string NormalizeAssignment(string line)
        {
            string trimmed = line.Trim();

            // If it already starts with "set", return as-is
            if (trimmed.ToLower().StartsWith("set "))
                return trimmed;

            // Add "set" prefix for BOOSE AppAsign
            return "set " + trimmed;
        }
    }

    /// <summary>
    /// Request model for execute endpoint
    /// </summary>
    public class ExecuteRequest
    {
        public string Commands { get; set; } = "";
    }
}