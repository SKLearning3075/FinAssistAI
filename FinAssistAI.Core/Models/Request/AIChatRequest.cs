using FinAssistAI.Core.Models.Common;
using FinAssistAI.Infrastructure.AI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Models.Request
{
    public class AIChatRequest
    {
        public List<ChatMessage> Messages { get; init; } = [];
        public string? SystemPrompt { get; init; }  = string.Empty;
        public bool Stream { get; init; } = false;
        public string? Model { get; init; }
    }
}
