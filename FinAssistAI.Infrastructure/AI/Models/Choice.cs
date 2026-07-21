using FinAssistAI.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Models
{
    public class Choice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = "";

        [JsonPropertyName("message")]
        public ChatMessage Message { get; set; } = new();
    }
}
