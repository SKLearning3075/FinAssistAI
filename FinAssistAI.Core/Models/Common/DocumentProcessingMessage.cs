using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Models.Common
{
    public class DocumentProcessingMessage
    {
        public Guid DocumentId { get; set; }

        public DateTime QueuedOn { get; set; } = DateTime.UtcNow;
    }
}
