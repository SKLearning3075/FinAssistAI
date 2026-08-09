using FinAssistAI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Models.Common
{
    public class Document
    {
        public Guid DocumentId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string OriginalFileName { get; set; } = string.Empty;

        public string StoredFilePath { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }

        public DocumentStatus Status { get; set; }

        public DateTime UploadedOn { get; set; }
    }
}
