using FinAssistAI.Core.Enums;
using FinAssistAI.Core.Interfaces.Repositories;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _repository;

        public ConversationService(
            IConversationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Conversation> GetOrCreateConversationAsync(Guid? conversationId, string userId)
        {
            if (conversationId.HasValue)
            {
                var conversation =
                    await _repository.GetByIdAsync(conversationId.Value);

                if (conversation != null)
                    return conversation;
            }

            var newConversation = new Conversation
            {
                ConversationId = Guid.NewGuid(),
                UserId = userId,
                Title = "New Conversation",
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            await _repository.AddAsync(newConversation);

            return newConversation;
        }

        public Task AddAssistantMessageAsync(Conversation conversation, string response, int promptTokens, int completionTokens, int totalTokens)
        {
            conversation.Messages.Add(new ConversationMessage
            {
                MessageId = Guid.NewGuid(),
                ConversationId = conversation.ConversationId,
                Role = MessageRole.Assistant,
                Content = response,
                PromptTokens = promptTokens,
                CompletionTokens = completionTokens,
                TotalTokens = totalTokens,
                CreatedOn = DateTime.UtcNow
            });

            conversation.UpdatedOn = DateTime.UtcNow;

            return Task.CompletedTask;
        }

        public Task AddUserMessageAsync(Conversation conversation, string message)
        {
            conversation.Messages.Add(new ConversationMessage
            {
                MessageId = Guid.NewGuid(),
                ConversationId = conversation.ConversationId,
                Role = MessageRole.User,
                Content = message,
                CreatedOn = DateTime.UtcNow
            });

            conversation.UpdatedOn = DateTime.UtcNow;

            return Task.CompletedTask;
        }
        
        public async Task SaveAsync(Conversation conversation)
        {
            await _repository.UpdateAsync(conversation);
        }
    }
}
