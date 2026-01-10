using Microsoft.AspNetCore.Mvc;
using BooseAPI.Models;
using MYBooseApp;

namespace BooseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooseController : ControllerBase
    {
        private readonly ILogger<BooseController> _logger;

        public BooseController(ILogger<BooseController> logger)
        {
            _logger = logger;
        }

        [HttpPost("execute")]
        public IActionResult ExecuteCode([FromBody] CodeRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Code))
                {
                    return BadRequest(new CodeResponse
                    {
                        Success = false,
                        Message = "No code provided"
                    });
                }

                // Get singleton handler
                var handler = AppCommandHandler.Instance;

                // Initialize with requested canvas size
                handler.Initialize(
                    request.CanvasWidth ?? 800,
                    request.CanvasHeight ?? 600
                );

                // Execute code
                var result = handler.ExecuteCode(request.Code);

                // Return response
                return Ok(new CodeResponse
                {
                    Success = result.Success,
                    Message = result.Message,
                    ImageBase64 = result.CanvasImage,
                    CommandCount = result.CommandCount,
                    ProgramStructure = handler.GetProgramStructure(),
                    ErrorDetails = result.ErrorDetails
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing BOOSE code");
                return StatusCode(500, new CodeResponse
                {
                    Success = false,
                    Message = "Internal server error",
                    ErrorDetails = ex.Message
                });
            }
        }

        [HttpPost("clear")]
        public IActionResult ClearCanvas()
        {
            try
            {
                var handler = AppCommandHandler.Instance;
                handler.ClearCanvas();

                return Ok(new CodeResponse
                {
                    Success = true,
                    Message = "Canvas cleared",
                    ImageBase64 = handler.GetCanvasAsBase64()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CodeResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("examples")]
        public IActionResult GetExamples()
        {
            var examples = new[]
            {
                new
                {
                    Name = "Simple Circle",
                    Code = @"pen 255,0,0
moveto 200,200
circle 50"
                },
                new
                {
                    Name = "For Loop Circles",
                    Code = @"pen 0,0,255
moveto 200,200
for count = 1 to 10 step 1
  circle count * 10
end for"
                },
                new
                {
                    Name = "Method Example",
                    Code = @"method int mulMethod int one, int two
  mulMethod = one * two
end method

call mulMethod 10 5
moveto 100,100
write mulMethod"
                }
            };

            return Ok(examples);
        }
    }
}