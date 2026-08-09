using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Models.Common
{
    public class SearchDocument
    {
        public string Id { get; set; } = default!;
        public Guid DocumentId { get; set; }
        public string FileName { get; set; } = default!;
        public string Department { get; set; } = default!;
        public string Country { get; set; } = default!;
        public int PageNumber { get; set; }
        public int ChunkNumber { get; set; }
        public string Content { get; set; } = default!;
        public float[] ContentVector { get; set; } = default!;
    }
}
