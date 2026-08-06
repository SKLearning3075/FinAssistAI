using FinAssistAI.Core.Commands;
using FinAssistAI.Core.Enums;
using FinAssistAI.Core.Interfaces.Repositories;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Common;
using FinAssistAI.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Services
{
    public class DocumentUploadOrchestrator
    {
        private readonly IDocumentStorageService _documentStorageService;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentProcessingQueue _processingQueue;
        public DocumentUploadOrchestrator(IDocumentStorageService documentStorageService, IDocumentRepository documentRepository, IDocumentProcessingQueue documentProcessingQueue)
        {
            this._documentRepository = documentRepository;
            this._documentStorageService = documentStorageService;
            this._processingQueue = documentProcessingQueue;
        }

        public async Task<UploadDocumentResult> UploadDocumentAsync(UploadDocumentCommand command, CancellationToken cancellationToken)
        {
            if (command.FileStream == null)
                throw new ArgumentNullException(nameof(command));

            if (string.IsNullOrWhiteSpace(command.FileName))
                throw new ArgumentException("File name is required.");

            var documentResponse = await _documentStorageService.SaveAsync(
            command.FileStream,
            command.FileName,
            cancellationToken);

            // Step 2 : Create Domain Model
            var document = new Document
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

            await _processingQueue.QueueAsync(
                new DocumentProcessingMessage
                {
                    DocumentId = document.DocumentId
                });

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
