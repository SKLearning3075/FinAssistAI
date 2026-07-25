using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Models.Response
{
    public class ChatResponse
    {
        public Guid ConversationId { get; set; }
        public string Answer { get; set; } = string.Empty;
    }
}
