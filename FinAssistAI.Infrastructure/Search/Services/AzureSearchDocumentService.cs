using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Common;
using FinAssistAI.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureSearchDocument =
    Azure.Search.Documents.Models.SearchDocument;

namespace FinAssistAI.Infrastructure.Search
{
    public class AzureSearchDocumentService : IAzureSearchDocumentService
    {
        private readonly SearchClient _searchClient;
        
        public AzureSearchDocumentService(SearchClient searchClient)
        {
            this._searchClient = searchClient;
        }
        public async Task<IReadOnlyList<SearchResult>> VectorSearchAsync(float[] queryVector, int topK = 5, CancellationToken cancellationToken = default)
        {
            var vectorQuery = new VectorizedQuery(queryVector)
            {
                KNearestNeighborsCount = topK,
                Fields =
            {
                "ContentVector"
            }
            };

            var searchOptions = new SearchOptions
            {
                Size = topK
            };

            searchOptions.VectorSearch = new VectorSearchOptions();

            searchOptions.VectorSearch.Queries.Add(vectorQuery);

            searchOptions.Select.Add("Id");
            searchOptions.Select.Add("DocumentId");
            searchOptions.Select.Add("Content");

            var response = await _searchClient.SearchAsync<AzureSearchDocument>(
                searchText: null,
                options: searchOptions,
                cancellationToken: cancellationToken);

            var results = new List<SearchResult>();

            await foreach (var result in response.Value.GetResultsAsync())
            {
                var document = result.Document;

                results.Add(new SearchResult
                {
                    Id = document.TryGetValue("Id", out var id)
                        ? id?.ToString() ?? string.Empty
                        : string.Empty,

                    DocumentId = document.TryGetValue("DocumentId", out var documentId)
                        ? documentId?.ToString() ?? string.Empty
                        : string.Empty,

                    Content = document.TryGetValue("Content", out var content)
                        ? content?.ToString() ?? string.Empty
                        : string.Empty,

                    Score = result.Score ?? 0
                });
            }

            return results;
        }
    }
}
