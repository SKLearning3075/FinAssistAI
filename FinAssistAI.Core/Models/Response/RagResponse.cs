using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Models.Response
{
    public class RagResponse
    {
        public string Question { get; set; } = string.Empty;

        public string Answer { get; set; } = string.Empty;

        public IReadOnlyList<SearchResult> Sources { get; set; }
            = Array.Empty<SearchResult>();
    }
}
