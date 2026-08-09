using FinAssistAI.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinAssistAI.Api.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IRagRetrievalService _retrievalService;

        public SearchController(
            IRagRetrievalService retrievalService)
        {
            _retrievalService = retrievalService;
        }

        [HttpGet("vector")]
        public async Task<IActionResult> VectorSearch(
       [FromQuery] string question,
       CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return BadRequest("Question is required.");
            }

            var results =
                await _retrievalService.RetrieveAsync(
                    question,
                    5,
                    cancellationToken);

            return Ok(results);
        }
    }
}
