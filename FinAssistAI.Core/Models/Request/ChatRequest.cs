using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Models.Request
{
    public class ChatRequest
    {
        public Guid? ConversationId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? SystemPrompt { get; set; } = string.Empty;
    }
}
