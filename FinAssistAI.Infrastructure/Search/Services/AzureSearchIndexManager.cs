using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using FinAssistAI.Infrastructure.AI.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Search.Services
{
    public class AzureSearchIndexManager
    {
        private readonly SearchIndexClient _indexClient;
        private readonly AzureSearchOptions _options;

        public AzureSearchIndexManager(
            IOptions<AzureSearchOptions> options)
        {
            _options = options.Value;

            _indexClient = new SearchIndexClient(
                new Uri(_options.Endpoint),
                new AzureKeyCredential(_options.ApiKey));
        }

        public async Task CreateIndexAsync(
            CancellationToken cancellationToken = default)
        {
            var vectorSearch = new VectorSearch
            {
                Algorithms =
            {
                new HnswAlgorithmConfiguration("hnsw")
            },

                Profiles =
            {
                new VectorSearchProfile(
                    "vector-profile",
                    "hnsw")
            }
            };

            var index = new SearchIndex(_options.IndexName)
            {
                VectorSearch = vectorSearch
            };

            index.Fields.Add(
                new SearchField(
                    "Id",
                    SearchFieldDataType.String)
                {
                    IsKey = true
                });

            index.Fields.Add(
                new SearchField(
                    "DocumentId",
                    SearchFieldDataType.String)
                {
                    IsFilterable = true
                });

            index.Fields.Add(
                new SearchField(
                    "FileName",
                    SearchFieldDataType.String)
                {
                    IsSearchable = true,
                    IsFilterable = true
                });

            index.Fields.Add(
                new SearchField(
                    "Department",
                    SearchFieldDataType.String)
                {
                    IsFilterable = true
                });

            index.Fields.Add(
                new SearchField(
                    "Country",
                    SearchFieldDataType.String)
                {
                    IsFilterable = true
                });

            index.Fields.Add(
                new SearchField(
                    "PageNumber",
                    SearchFieldDataType.Int32)
                {
                    IsFilterable = true
                });

            index.Fields.Add(
                new SearchField(
                    "ChunkNumber",
                    SearchFieldDataType.Int32)
                {
                    IsFilterable = true
                });

            index.Fields.Add(
                new SearchField(
                    "Content",
                    SearchFieldDataType.String)
                {
                    IsSearchable = true
                });

            index.Fields.Add(
                new SearchField(
                    "ContentVector",
                    SearchFieldDataType.Collection(
                        SearchFieldDataType.Single))
                {
                    IsSearchable = true,

                    VectorSearchDimensions = 1536,

                    VectorSearchProfileName =
                        "vector-profile"
                });

            await _indexClient.CreateOrUpdateIndexAsync(
                index,
                cancellationToken: cancellationToken);
        }
    }
}
