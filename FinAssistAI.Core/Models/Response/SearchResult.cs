using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Models.Response
{
    public class SearchResult
    {
        public string Id { get; set; } = string.Empty;

        public string DocumentId { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public double Score { get; set; }
    }
}
