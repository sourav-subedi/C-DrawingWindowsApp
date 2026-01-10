using BOOSE;
using System;
using System.IO;

namespace MYBooseApp
{
    /// <summary>
    /// Singleton command handler for executing BOOSE commands and managing application operations.
    /// </summary>
    public class AppCommandHandler
    {
        private static AppCommandHandler instance;
        private static readonly object lockObject = new object();

        private AppCanvas canvas;
        private AppCommandFactory factory;
        private AppStoredProgram program;
        private AppParser parser;

        /// <summary>
        /// Gets the singleton instance of the command handler.
        /// </summary>
        public static AppCommandHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AppCommandHandler();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Gets the current canvas instance.
        /// </summary>
        public AppCanvas Canvas => canvas;

        /// <summary>
        /// Gets the current program instance.
        /// </summary>
        public AppStoredProgram Program => program;

        /// <summary>
        /// Private constructor to enforce singleton pattern.
        /// </summary>
        private AppCommandHandler()
        {
            // Initialize with default canvas size
            Initialize(800, 600);
        }

        /// <summary>
        /// Initializes or reinitializes the BOOSE environment with specified canvas dimensions.
        /// </summary>
        public void Initialize(int width, int height)
        {
            canvas = new AppCanvas(width, height);
            factory = new AppCommandFactory();
            program = new AppStoredProgram(canvas);
            parser = new AppParser(factory, program);

            canvas.Clear();
        }

        /// <summary>
        /// Executes BOOSE code and returns the execution result.
        /// </summary>
        public ExecutionResult ExecuteCode(string code)
        {
            var result = new ExecutionResult();

            try
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    result.Success = false;
                    result.Message = "No code provided";
                    return result;
                }

                // Clear canvas and reset program
                canvas.Clear();
                program.ResetProgram();

                // Parse and execute
                parser.ParseProgram(code);
                result.CommandCount = program.Count;

                if (program.Count == 0)
                {
                    result.Success = false;
                    result.Message = "No commands were parsed";
                    return result;
                }

                program.Run();

                result.Success = true;
                result.Message = $"Successfully executed {program.Count} commands";
                result.CanvasImage = GetCanvasAsBase64();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
                result.ErrorDetails = ex.StackTrace;
            }

            return result;
        }

        /// <summary>
        /// Gets the current canvas as a Base64-encoded PNG image.
        /// </summary>
        public string GetCanvasAsBase64()
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    canvas.getBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Saves the current canvas to a file.
        /// </summary>
        public bool SaveCanvasToFile(string filePath, string format = "png")
        {
            try
            {
                var imageFormat = format.ToLower() == "jpg" || format.ToLower() == "jpeg"
                    ? System.Drawing.Imaging.ImageFormat.Jpeg
                    : System.Drawing.Imaging.ImageFormat.Png;

                canvas.getBitmap().Save(filePath, imageFormat);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Saves BOOSE code to a file.
        /// </summary>
        public bool SaveCodeToFile(string filePath, string code)
        {
            try
            {
                File.WriteAllText(filePath, code);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Loads BOOSE code from a file.
        /// </summary>
        public string LoadCodeFromFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Clears the canvas to blank state.
        /// </summary>
        public void ClearCanvas()
        {
            canvas.Clear();
        }

        /// <summary>
        /// Resets the entire program state.
        /// </summary>
        public void ResetProgram()
        {
            program.ResetProgram();
            canvas.Clear();
        }

        /// <summary>
        /// Gets program structure information for debugging.
        /// </summary>
        public string[] GetProgramStructure()
        {
            var structure = new string[program.Count];
            for (int i = 0; i < program.Count; i++)
            {
                structure[i] = $"Line {i}: {program[i].GetType().Name}";
            }
            return structure;
        }
    }

    /// <summary>
    /// Represents the result of a code execution operation.
    /// </summary>
    public class ExecutionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ErrorDetails { get; set; }
        public int CommandCount { get; set; }
        public string CanvasImage { get; set; }
    }
}