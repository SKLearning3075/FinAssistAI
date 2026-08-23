using FinAssistAI.Core.Interfaces.Repositories;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Common;
using FinAssistAI.Infrastructure.AI.Services;
using FinAssistAI.Infrastructure.Processing;
using FinAssistAI.Infrastructure.Repositories;
using FinAssistAI.ServiceBusFunction.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.ServiceBusFunction.Services
{
    public class ServiceBusProcessDocumentService : IServiceBusProcessDocument
    {
        private readonly ILogger<ServiceBusProcessDocumentService> _logger;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentProcessEventRepository _documentProcessEventRepository;
        private readonly ITextExtractionService _textExtractionService;
        private readonly IChunkingService _chunkingService;
        private readonly IEmbeddingService _embeddingService;
        private readonly ISearchIndexService _searchIndexService;

        public ServiceBusProcessDocumentService(ILogger<ServiceBusProcessDocumentService> logger, IDocumentRepository documentRepository, IDocumentProcessEventRepository documentProcessEventRepository, ITextExtractionService textExtractionService, IChunkingService chunkingService, IEmbeddingService embeddingService, ISearchIndexService searchIndexService
            )
        {
            _logger = logger;
            _documentRepository = documentRepository;
            _documentProcessEventRepository = documentProcessEventRepository;
            _textExtractionService = textExtractionService;
            _chunkingService = chunkingService;
            _embeddingService = embeddingService;
            _searchIndexService = searchIndexService;
        }
        public async Task ProcessDocumentAsync(Guid documentId, Guid eventId, string idempotencyKey, CancellationToken cancellationToken)
        {
            try
            {
                var document = await _documentRepository.GetByIdAsync(documentId);

                if (document == null)
                {
                    _logger.LogWarning($"Document with ID: {documentId} not found.");
                    return;
                }
                DocumentProcessEvent documentProcessEvent = new DocumentProcessEvent
                {
                    IdempotencyKey = idempotencyKey,
                    EventId = eventId,
                    ProcessedAt = DateTime.UtcNow
                };

                if (_documentProcessEventRepository.IsExist(documentProcessEvent.IdempotencyKey))
                {
                    _logger.LogInformation($"Document with ID: {documentId} has already been processed. Skipping.");
                    return;
                }
                else
                {
                    await _documentProcessEventRepository.AddAsync(documentProcessEvent);
                }

                var pdfText = await _textExtractionService.ExtractTextAsync(document.StoredFilePath, cancellationToken);
                var chunksResult = await _chunkingService.ChunkAsync(pdfText, cancellationToken);

                for (int i = 0; i < chunksResult.Count; i++)
                {
                    var embedding = await _embeddingService.GenerateEmbeddingAsync(
                                            chunksResult[i],
                                            cancellationToken);

                    var searchDocument = new SearchDocument
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = document.DocumentId,
                        FileName = document.FileName,
                        Department = document.Department,
                        Country = document.Country,
                        PageNumber = 1,
                        ChunkNumber = i + 1,
                        Content = chunksResult[i],
                        ContentVector = embedding
                    };

                    await _searchIndexService.IndexDocumentAsync(
                                              searchDocument,
                                              cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing document with ID: {documentId}");
                throw;
            }
            
        }
    }
}
