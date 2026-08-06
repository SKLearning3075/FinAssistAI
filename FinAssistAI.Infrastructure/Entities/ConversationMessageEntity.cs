using FinAssistAI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Entities
{
    public class ConversationMessageEntity
    {
        public Guid MessageId { get; set; }
        public Guid ConversationId { get; set; }
        public MessageRole Role { get; set; }
        public string Content { get; set; } = string.Empty;
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
        public ConversationEntity Conversation { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
