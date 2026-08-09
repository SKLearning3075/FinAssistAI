using FinAssistAI.Core.Interfaces.Repositories;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Common;
using FinAssistAI.Infrastructure.AI.Services;
using FinAssistAI.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Processing
{
    public class DocumentProcessor : IDocumentProcessor
    {
        private readonly ILogger<DocumentProcessor> _logger;
        private readonly ITextExtractionService _textExtractionService;
        private readonly IDocumentRepository _documentRepository;
        private readonly IChunkingService _chunkingService;
        private readonly IEmbeddingService _embeddingService;
        private readonly ISearchIndexService _searchIndexService;

        public DocumentProcessor(ITextExtractionService textExtractionService,
            ILogger<DocumentProcessor> logger, IDocumentRepository documentRepository, IChunkingService chunkingService, IEmbeddingService embeddingService, ISearchIndexService searchIndexService)
        {
            _logger = logger;
            _textExtractionService = textExtractionService;
            _documentRepository = documentRepository;
            _chunkingService = chunkingService;
            _embeddingService = embeddingService;
            _searchIndexService = searchIndexService;
        }
        public async Task ProcessAsync(Guid documentId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Processing document with ID: {documentId}");

            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
                throw new Exception("Document not found.");

           var pdfText =  await _textExtractionService.ExtractTextAsync(document.StoredFilePath, cancellationToken);

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
            
            _logger.LogInformation("Generated {Count} chunks", chunksResult.Count);

            _logger.LogInformation($"Finished processing document with ID: {documentId}");
        }
    }
}
