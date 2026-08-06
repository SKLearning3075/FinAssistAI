using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Models.Request
{
    public class UploadDocumentCommand
    {
        public string UserId { get; set; } = string.Empty;

        public Stream FileStream { get; set; } = null!;

        public string FileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;
    }
}
