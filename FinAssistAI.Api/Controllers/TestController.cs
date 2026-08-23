using FinAssistAI.Contracts.Events;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Infrastructure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace FinAssistAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMessagePublisher _messagePublisher;

        public TestController(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        [HttpPost("servicebus-test")]
        public async Task<IActionResult> SendServiceBusMessage()
        {
            var correlationId = Guid.NewGuid().ToString();
            var documentId = Guid.NewGuid();

            var documentUploadedEvent = new DocumentUploadedEvent
            {
                DocumentId = documentId,
                CorrelationId = correlationId,
                IdempotencyKey = $"DocumentUploaded:{documentId}"
            };

            await _messagePublisher.PublishAsync(
                documentUploadedEvent,
                CancellationToken.None);

            return Ok(documentUploadedEvent);
        }
    }
}
