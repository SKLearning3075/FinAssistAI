using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Configuration
{
    public class AzureServiceBusOptions
    {
        public string FullyQualifiedNamespace { get; set; }
        public string QueueName { get; set; }
        public string TanantId { get; set; }
    }
}
