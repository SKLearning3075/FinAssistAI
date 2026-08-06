using FinAssistAI.Core.Interfaces.Repositories;
using FinAssistAI.Core.Interfaces.Services;
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

        public DocumentProcessor(ITextExtractionService textExtractionService,
            ILogger<DocumentProcessor> logger, IDocumentRepository documentRepository, IChunkingService chunkingService)
        {
            _logger = logger;
            _textExtractionService = textExtractionService;
            _documentRepository = documentRepository;
            _chunkingService = chunkingService;
        }
        public async Task ProcessAsync(Guid documentId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Processing document with ID: {documentId}");

            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
                throw new Exception("Document not found.");

           var pdfText =  await _textExtractionService.ExtractTextAsync(document.StoredFilePath, cancellationToken);

           var chunksResult = await _chunkingService.ChunkAsync(pdfText, cancellationToken);

            _logger.LogInformation("Generated {Count} chunks", chunksResult.Count);

            _logger.LogInformation($"Finished processing document with ID: {documentId}");
        }
    }
}
