using FinAssistAI.Api.Models;
using FinAssistAI.Core.Commands;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinAssistAI.Api.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentUploadOrchestrator _orchestrator;
        public DocumentController(DocumentUploadOrchestrator uploadOrchestratorService)
        {
            _orchestrator = uploadOrchestratorService;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] UploadDocumentRequest request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var command = new UploadDocumentCommand
            {
                UserId = request.UserId,
                FileName = request.File.FileName,
                ContentType = request.File.ContentType,
                FileSize = request.File.Length,
                FileStream = request.File.OpenReadStream()
            };

            var result = await _orchestrator.UploadDocumentAsync(
            command,
            cancellationToken);

            return Ok(result);
        }
    }
}
