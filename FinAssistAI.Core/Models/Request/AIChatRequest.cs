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
        public IReadOnlyCollection<ChatMessage> Messages { get; init; } = [];

        public double Temperature { get; init; } = 0.7;

        public int MaxTokens { get; init; } = 1000;

        public bool Stream { get; init; } = false;

        public string? Model { get; init; }
    }
}
