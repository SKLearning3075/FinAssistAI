using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Configuration
{
    public class AzureBlobOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string ContainerName { get; set; } = "finassist-document";
    }
}
