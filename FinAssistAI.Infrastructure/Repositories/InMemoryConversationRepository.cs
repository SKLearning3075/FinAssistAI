using FinAssistAI.Core.Interfaces.Repositories;
using FinAssistAI.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Repositories
{
    public class InMemoryConversationRepository : IConversationRepository
    {
        private static readonly List<Conversation> _conversations = new();

        public Task AddAsync(Conversation conversation)
        {
            _conversations.Add(conversation);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid conversationId)
        {
            var conversation = _conversations
                .FirstOrDefault(c => c.ConversationId == conversationId);

            if (conversation != null)
            {
                _conversations.Remove(conversation);
            }

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Guid conversationId)
        {
            return Task.FromResult(
                _conversations.Any(c => c.ConversationId == conversationId));
        }

        public Task<List<Conversation>> GetAllAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Conversation?> GetByIdAsync(Guid conversationId)
        {
            var conversation = _conversations
                .FirstOrDefault(c => c.ConversationId == conversationId);

            return Task.FromResult(conversation);
        }

        public Task<List<Conversation>> GetByUserAsync(string userId)
        {
            var conversations = _conversations
                .Where(c => c.UserId == userId)
                .ToList();

            return Task.FromResult(conversations);
        }

        public Task UpdateAsync(Conversation conversation)
        {
            var existingConversation = _conversations
                .FirstOrDefault(c => c.ConversationId == conversation.ConversationId);

            if (existingConversation != null)
            {
                _conversations.Remove(existingConversation);
            }

            _conversations.Add(conversation);

            return Task.CompletedTask;
        }
    }
}
