using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Configuration
{
    public class AzureOpenAIOptions
    {
        public string Endpoint { get; set; } = string.Empty;

        public string ApiKey { get; set; } = string.Empty;

        public string EmbeddingDeployment { get; set; } = string.Empty;

        public string ChatDeployment { get; set; } = string.Empty;
    }
}
