using FinAssistAI.Core.Models.Common;

namespace FinAssistAI.Core.Interfaces.Repositories
{
    public interface IConversationRepository
    {
        Task<Conversation?> GetByIdAsync(Guid conversationId);

        Task<List<Conversation>> GetAllAsync(string userId);

        Task AddAsync(Conversation conversation);

        Task UpdateAsync(Conversation conversation);

        Task DeleteAsync(Guid conversationId);

    }
}
