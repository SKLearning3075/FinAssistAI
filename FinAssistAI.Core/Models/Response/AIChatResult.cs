using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Models
{
    public class AIChatResult
    {
        public string Content { get; init; } = string.Empty;

        public string Model { get; init; } = string.Empty;

        public int PromptTokens { get; init; }

        public int CompletionTokens { get; init; }

        public int TotalTokens { get; init; }
    }
}
