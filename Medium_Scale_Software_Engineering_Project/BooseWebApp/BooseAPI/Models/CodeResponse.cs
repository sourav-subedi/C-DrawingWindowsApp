namespace BooseAPI.Models
{
    public class CodeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ImageBase64 { get; set; }
        public int CommandCount { get; set; }
        public string[] ProgramStructure { get; set; }
        public string ErrorDetails { get; set; }
    }
}