using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Models.Response
{
    public class UploadDocumentResult: UploadDocument
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
