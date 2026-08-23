using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Models.Common
{
    public class DocumentProcessEvent
    {
        public string IdempotencyKey { get; set; } = string.Empty;
        public Guid EventId { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
