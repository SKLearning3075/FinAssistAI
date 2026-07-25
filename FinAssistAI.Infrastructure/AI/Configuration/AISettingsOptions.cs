using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Configuration
{
    public class AISettingsOptions
    {
        public double Temperature { get; set; }
        public int MaxTokens { get; set; }
        public string TopP { get; set; } = string.Empty;
        public string FrequencyPenalty { get; set; } = string.Empty;
        public string PresencePenalty { get; set; } = string.Empty;
    }
}
