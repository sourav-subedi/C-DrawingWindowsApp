namespace BooseAPI.Models
{
    public class CodeRequest
    {
        public string Code { get; set; }
        public int? CanvasWidth { get; set; } = 800;
        public int? CanvasHeight { get; set; } = 600;
    }
}