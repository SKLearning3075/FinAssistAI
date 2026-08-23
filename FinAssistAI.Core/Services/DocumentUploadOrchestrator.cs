using FinAssistAI.Core.Commands;
using FinAssistAI.Core.Enums;
using FinAssistAI.Core.Interfaces.Repositories;
using FinAssistAI.Core.Interfaces.Services;
using model = FinAssistAI.Core.Models.Common;
using FinAssistAI.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using FinAssistAI.Contracts.Events;

namespace FinAssistAI.Core.Services
{
    public class DocumentUploadOrchestrator
    {
        private readonly IDocumentStorageService _documentStorageService;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentProcessingQueue _processingQueue;
        private readonly IMessagePublisher _messagePublisher;

        public DocumentUploadOrchestrator(IDocumentStorageService documentStorageService, IDocumentRepository documentRepository, IDocumentProcessingQueue documentProcessingQueue, IMessagePublisher messagePublisher)
        {
            this._documentRepository = documentRepository;
            this._documentStorageService = documentStorageService;
            this._processingQueue = documentProcessingQueue;
            this._messagePublisher = messagePublisher;
        }

        public async Task<UploadDocumentResult> UploadDocumentAsync(UploadDocumentCommand command, CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid().ToString();
            if (command.FileStream == null)
                throw new ArgumentNullException(nameof(command));

            if (string.IsNullOrWhiteSpace(command.FileName))
                throw new ArgumentException("File name is required.");

            var documentResponse = await _documentStorageService.SaveAsync(
            command.FileStream,
            command.FileName,
            cancellationToken);

            // Step 2 : Create Domain Model
            var document = new model.Document
            {
                DocumentId = Guid.NewGuid(),
                UserId = command.UserId,
                OriginalFileName = command.FileName,
                StoredFilePath = documentResponse.FilePath,
                ContentType = command.ContentType,
                FileSize = command.FileSize,
                Status = DocumentStatus.Uploaded,
            };

            // Step 3 : Save metadata
            await _documentRepository.AddAsync(document);

            DocumentUploadedEvent documentUploadedEvent = new DocumentUploadedEvent
            {
                DocumentId = document.DocumentId,
                CorrelationId = correlationId,
                IdempotencyKey = $"DocumentUploaded:{document.DocumentId.ToString()}",
                Timestamp = DateTimeOffset.UtcNow
            };
            await _messagePublisher.PublishAsync(documentUploadedEvent);
            
            return new UploadDocumentResult
            {
                Success = true,
                FileName = documentResponse.FileName,
                FilePath = documentResponse.FilePath,
                Message = "Document uploaded successfully."
            };
        }
    }
}
