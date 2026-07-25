using FinAssistAI.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Interfaces.Repositories
{
    public interface IConversationRepository
    {
        Task<Conversation?> GetByIdAsync(Guid conversationId);

        Task<List<Conversation>> GetByUserAsync(string userId);

        Task CreateAsync(Conversation conversation);

        Task UpdateAsync(Conversation conversation);

        Task DeleteAsync(Guid conversationId);

        Task<bool> ExistsAsync(Guid conversationId);
    }
}
