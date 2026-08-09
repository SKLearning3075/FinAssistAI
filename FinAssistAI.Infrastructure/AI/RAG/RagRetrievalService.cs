using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.RAG
{
    public class RagRetrievalService : IRagRetrievalService
    {
        
        private readonly IEmbeddingService _embeddingService;
        private readonly IAzureSearchDocumentService _searchService;

        public RagRetrievalService(
            IEmbeddingService embeddingService,
            IAzureSearchDocumentService searchService)
        {
            _embeddingService = embeddingService;
            _searchService = searchService;
        }

        public async Task<IReadOnlyList<SearchResult>> RetrieveAsync(
            string question,
            int topK = 5,
            CancellationToken cancellationToken = default)
        {
            var queryVector =
                await _embeddingService.GenerateEmbeddingAsync(
                    question,
                    cancellationToken);

            return await _searchService.VectorSearchAsync(
                queryVector,
                topK,
                cancellationToken);
        }
    }

}
