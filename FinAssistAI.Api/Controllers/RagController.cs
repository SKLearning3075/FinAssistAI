using FinAssistAI.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinAssistAI.Api.Controllers
{
    [Route("api/rag")]
    [ApiController]
    public class RagController : ControllerBase
    {
        private readonly IRagService _ragService;

        public RagController(IRagService ragService)
        {
            _ragService = ragService;
        }

        [HttpGet("ask")]
        public async Task<IActionResult> Ask(
        [FromQuery] string question,
        CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return BadRequest("Question is required.");
            }

            var response =
                await _ragService.AskAsync(
                    question,
                    cancellationToken);

            return Ok(response);
        }
    }
}
