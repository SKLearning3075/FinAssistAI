using FinAssistAI.Core.Interfaces.Repositories;
using FinAssistAI.Core.Models.Common;
using FinAssistAI.Infrastructure.Entities;
using FinAssistAI.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly FinAssistDbContext _dbContext;
        public DocumentRepository(FinAssistDbContext dbContext) { 
            this._dbContext = dbContext;
        }
        public Task AddAsync(Document document)
        {
            var documentEntity = MapToEntity(document);
            var entityEntry = _dbContext.Documents.Add(documentEntity);

            _dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<Document?> GetByIdAsync(Guid documentId)
        {
            var documentEntity = _dbContext.Documents
                .FirstOrDefault(d => d.DocumentId == documentId);
            if (documentEntity == null)
            {
                return Task.FromResult<Document?>(null);
            }
            var document = MapToDomain(documentEntity);
            
            return Task.FromResult<Document?>(document);
        }
        

        public Task UpdateAsync(Document document)
        {
            var documentEntity = _dbContext.Documents
                .FirstOrDefault(d => d.DocumentId == document.DocumentId);
            if (documentEntity == null)
            {
                throw new InvalidOperationException($"Document with ID {document.DocumentId} not found.");
            }
            // Update the properties of the entity
            documentEntity.OriginalFileName = document.OriginalFileName;
            documentEntity.StoredFilePath = document.StoredFilePath;
            documentEntity.ContentType = document.ContentType;
            documentEntity.FileSize = document.FileSize;
            documentEntity.Status = document.Status;
            documentEntity.UploadedOn = document.UploadedOn;
            _dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        #region Mapping

        private static Document MapToDomain(
            DocumentEntity entity)
        {
            return new Document
            {
                DocumentId = entity.DocumentId,
                UserId = entity.UserId,
                OriginalFileName = entity.OriginalFileName,
                StoredFilePath = entity.StoredFilePath,
                ContentType = entity.ContentType,
                FileSize = entity.FileSize,
                Status = entity.Status,
                UploadedOn = entity.UploadedOn
            };
        }
        

        private static DocumentEntity MapToEntity(
           Document conversation)
        {
            return new DocumentEntity
            {
                DocumentId = conversation.DocumentId,
                UserId = conversation.UserId,
                OriginalFileName = conversation.OriginalFileName,
                StoredFilePath = conversation.StoredFilePath,
                ContentType = conversation.ContentType,
                FileSize = conversation.FileSize,
                Status = conversation.Status,
                UploadedOn = conversation.UploadedOn
            };
        }

        #endregion
    }
}
