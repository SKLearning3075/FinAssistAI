using Azure;
using Azure.AI.OpenAI;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Infrastructure.AI.Configuration;
using Microsoft.Extensions.Options;
using OpenAI.Embeddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Services
{
    public class AzureOpenAIEmbeddingService : IEmbeddingService
    {
        private readonly AzureOpenAIOptions _options;
        private readonly EmbeddingClient _embeddingClient;
        public AzureOpenAIEmbeddingService(IOptions<AzureOpenAIOptions> configureOptions)
        {
            _options = configureOptions.Value;

            if (string.IsNullOrWhiteSpace(_options.Endpoint))
                throw new InvalidOperationException(
                    "Azure OpenAI Endpoint is not configured.");

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                throw new InvalidOperationException(
                    "Azure OpenAI ApiKey is not configured.");

            if (string.IsNullOrWhiteSpace(_options.EmbeddingDeployment))
                throw new InvalidOperationException(
                    "Azure OpenAI EmbeddingDeployment is not configured.");

            var azureOpenAIClient = new AzureOpenAIClient(
                new Uri(_options.Endpoint),
                new AzureKeyCredential(_options.ApiKey));

            _embeddingClient = azureOpenAIClient.GetEmbeddingClient(_options.EmbeddingDeployment);
        }
        public async Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    "Text cannot be empty.",
                    nameof(text));
            }

            var result = await _embeddingClient.GenerateEmbeddingAsync(
                    text,
                    cancellationToken: cancellationToken);
            
            return result.Value
                            .ToFloats()
                            .ToArray();
        }
    }
}
