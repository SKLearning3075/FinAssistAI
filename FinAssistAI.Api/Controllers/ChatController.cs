using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace FinAssistAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService) { 
        this._chatService = chatService;
        }

        [Route("ask")]
        [HttpPost]
        public async Task<IActionResult> AskAsync(
        [FromBody] ChatRequest request,
        CancellationToken cancellationToken)
        {
            var response = await _chatService.AskAsync(
            request,
            cancellationToken);

            return Ok(response);

        }
    }
}
