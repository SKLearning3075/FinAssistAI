using FinAssistAI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Entities
{
    public class DocumentEntity
    {
        public Guid DocumentId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string OriginalFileName { get; set; } = string.Empty;

        public string StoredFilePath { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DocumentStatus Status { get; set; }

        public DateTime UploadedOn { get; set; }
    }
}
