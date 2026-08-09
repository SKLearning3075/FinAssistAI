using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Configuration
{
    public class AzureSearchOptions
    {
        public string Endpoint { get; set; } = default!;

        public string ApiKey { get; set; } = default!;

        public string IndexName { get; set; } = default!;
    }
}
