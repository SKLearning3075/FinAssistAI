using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Contracts.Events
{
    public class DocumentUploadedEvent
    {
        public string EventType { get; init; } = "DocumentUploaded";
        public Guid EventId { get; init; } = Guid.NewGuid();
        public Guid DocumentId { get; init; }
        public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
        public string CorrelationId { get; init; } = default!;
        public string IdempotencyKey { get; init; } = default!;
    }
}
