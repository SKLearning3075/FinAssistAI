using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Commands
{
    public class UploadDocumentCommand
    {
        public string UserId { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public Stream FileStream { get; set; } = Stream.Null;
        public long FileSize { get; set; }
    }
}
