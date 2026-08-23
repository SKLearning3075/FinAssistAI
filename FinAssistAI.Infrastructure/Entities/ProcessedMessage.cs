using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Entities
{
    public class ProcessedMessage
    {
        public string IdempotencyKey { get; set; } = default!;
        public Guid EventId { get; set; } = default!;
        public DateTime ProcessedAt { get; set; }
    }
}
