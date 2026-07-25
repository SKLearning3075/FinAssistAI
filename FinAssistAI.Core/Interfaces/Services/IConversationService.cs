using FinAssistAI.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Interfaces.Services
{
    public interface IConversationService
    {
        Task<Conversation> GetOrCreateConversationAsync(
        Guid? conversationId,
        string userId);

        Task AddUserMessageAsync(
            Conversation conversation,
            string message);

        Task AddAssistantMessageAsync(
            Conversation conversation,
            string response,
            int promptTokens,
            int completionTokens,
            int totalTokens);

        Task SaveAsync(Conversation conversation);
    }
}
