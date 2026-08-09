using Azure.Search.Documents;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Common;
using FinAssistAI.Infrastructure.AI.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Search.Services
{
    public class AzureSearchIndexService : ISearchIndexService
    {
        private readonly SearchClient _searchClient;
        public AzureSearchIndexService(IOptions<AzureSearchOptions> option) {
            var setting = option.Value;

            _searchClient = new SearchClient(
                new Uri(setting.Endpoint),
                setting.IndexName,
                new Azure.AzureKeyCredential(setting.ApiKey));
        }

        public async Task IndexDocumentAsync(SearchDocument document, CancellationToken cancellationToken)
        {
            await _searchClient.UploadDocumentsAsync(
                                new[] { document });
        }
    }
}
