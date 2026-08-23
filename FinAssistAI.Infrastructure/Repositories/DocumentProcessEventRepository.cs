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
    public class DocumentProcessEventRepository : IDocumentProcessEventRepository
    {
        private readonly FinAssistDbContext _dbContext;
        public DocumentProcessEventRepository(FinAssistDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }
        public Task AddAsync(DocumentProcessEvent documentProcessEvent)
        {
            ProcessedMessage processedMessage = new ProcessedMessage
            {
                IdempotencyKey = documentProcessEvent.IdempotencyKey,
                EventId = documentProcessEvent.EventId,
                ProcessedAt = documentProcessEvent.ProcessedAt
            };
            _dbContext.ProcessedMessages.Add(processedMessage);
            return _dbContext.SaveChangesAsync();
        }

        public Task<DocumentProcessEvent?> GetByIdAsync(Guid idempotencyKey)
        {
           var processedMessage = _dbContext.ProcessedMessages.FirstOrDefault(pm => pm.IdempotencyKey == idempotencyKey.ToString());
           
            DocumentProcessEvent? documentProcessEvent = null;
            if (processedMessage != null) {
                documentProcessEvent = new DocumentProcessEvent
                {
                    IdempotencyKey = processedMessage.IdempotencyKey,
                    EventId = processedMessage.EventId,
                    ProcessedAt = processedMessage.ProcessedAt
                };

            }
            return Task.FromResult(documentProcessEvent);
        }

        public bool IsExist(string idempotencyKey)
        {
           return _dbContext.ProcessedMessages.Any(pm => pm.IdempotencyKey == idempotencyKey);
        }

        public Task UpdateAsync(DocumentProcessEvent documentProcessEvent)
        {
            throw new NotImplementedException();
        }
    }
}
