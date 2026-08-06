using FinAssistAI.Core.Interfaces.Repositories;
using FinAssistAI.Core.Models.Common;
using FinAssistAI.Infrastructure.Entities;
using FinAssistAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Repositories
{
    public class EFConversationRepository : IConversationRepository
    {
        private readonly FinAssistDbContext _dbContext;

        public EFConversationRepository(
            FinAssistDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Conversation?> GetByIdAsync(Guid conversationId)
        {
            var entity = await _dbContext.Conversations
                .Include(x => x.Messages)
                .FirstOrDefaultAsync(x => x.ConversationId == conversationId);

            if (entity == null)
                return null;

            return MapToDomain(entity);
        }

        public async Task<List<Conversation>> GetAllAsync(string userId)
        {
            var conversations = await _dbContext.Conversations
                .Include(x => x.Messages)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return conversations
                .Select(MapToDomain)
                .ToList();
        }

        public async Task AddAsync(Conversation conversation)
        {
            var entity =  new ConversationEntity
            {
                ConversationId = conversation.ConversationId,
                UserId = conversation.UserId,
                Title = conversation.Title,
                CreatedOn = conversation.CreatedOn,
                UpdatedOn = conversation.UpdatedOn,

                Messages = conversation.Messages
                    .Select(x => new ConversationMessageEntity
                    {
                        MessageId = x.MessageId,
                        ConversationId = x.ConversationId,
                        Role = x.Role,
                        Content = x.Content,
                        PromptTokens = x.PromptTokens,
                        CompletionTokens = x.CompletionTokens,
                        TotalTokens = x.TotalTokens,
                        CreatedOn = x.CreatedOn
                    })
                    .ToList()
            };

            await _dbContext.Conversations.AddAsync(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Conversation conversation)
        {
            var entity = await _dbContext.Conversations
                                         //.Include(c => c.Messages)
                                         .FirstOrDefaultAsync(c => c.ConversationId == conversation.ConversationId);

            if (entity == null)
                throw new Exception("Conversation not found.");

            // Parent properties
            entity.Title = conversation.Title;
            entity.UpdatedOn = conversation.UpdatedOn;


            // Add only NEW messages
            foreach (var message in conversation.Messages)
            {
                bool exists = _dbContext.ConversationMessages.Any(x => x.MessageId == message.MessageId);

                if (!exists)
                {

                var messageentity = new ConversationMessageEntity
                    {
                        MessageId = message.MessageId,
                        ConversationId = entity.ConversationId,
                        Role = message.Role,
                        Content = message.Content,
                        PromptTokens = message.PromptTokens,
                        CompletionTokens = message.CompletionTokens,
                        TotalTokens = message.TotalTokens,
                        CreatedOn = message.CreatedOn,
                        UpdatedOn = message.UpdatedOn
                    };

                    _dbContext.ConversationMessages.Add(messageentity);
                }
                else
                {
                    // Optionally, update existing message properties if needed
                    var existingMessage = await _dbContext.ConversationMessages
                        .FirstOrDefaultAsync(x => x.MessageId == message.MessageId);
                    if (existingMessage != null)
                    {
                        existingMessage.Role = message.Role;
                        existingMessage.Content = message.Content;
                        existingMessage.PromptTokens = message.PromptTokens;
                        existingMessage.CompletionTokens = message.CompletionTokens;
                        existingMessage.TotalTokens = message.TotalTokens;
                        existingMessage.UpdatedOn = message.UpdatedOn;
                    }
                }
            }
            
           await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid conversationId)
        {
            var entity = await _dbContext.Conversations
                .FirstOrDefaultAsync(x => x.ConversationId == conversationId);

            if (entity == null)
                return;

            _dbContext.Conversations.Remove(entity);

            await _dbContext.SaveChangesAsync();
        }


        #region Mapping

        private static Conversation MapToDomain(
            ConversationEntity entity)
        {
            return new Conversation
            {
                ConversationId = entity.ConversationId,
                UserId = entity.UserId,
                Title = entity.Title,
                CreatedOn = entity.CreatedOn,
                UpdatedOn = entity.UpdatedOn,

                Messages = entity.Messages
                    .Select(x => new ConversationMessage
                    {
                        MessageId = x.MessageId,
                        ConversationId = x.ConversationId,
                        Role = x.Role,
                        Content = x.Content,
                        PromptTokens = x.PromptTokens,
                        CompletionTokens = x.CompletionTokens,
                        TotalTokens = x.TotalTokens,
                        CreatedOn = x.CreatedOn
                    })
                    .ToList()
            };
        }

        //private static ConversationEntity MapToEntity(
        //   Conversation conversation)
        //{
        //    return new ConversationEntity
        //    {
        //        ConversationId = conversation.ConversationId,
        //        UserId = conversation.UserId,
        //        Title = conversation.Title,
        //        CreatedOn = conversation.CreatedOn,
        //        UpdatedOn = conversation.UpdatedOn,

        //        Messages = conversation.Messages
        //            .Select(x => new ConversationMessageEntity
        //            {
        //                MessageId = x.MessageId,
        //                ConversationId = x.ConversationId,
        //                Role = x.Role,
        //                Content = x.Content,
        //                PromptTokens = x.PromptTokens,
        //                CompletionTokens = x.CompletionTokens,
        //                TotalTokens = x.TotalTokens,
        //                CreatedOn = x.CreatedOn
        //            })
        //            .ToList()
        //    };
        //}

        #endregion
    }
}
