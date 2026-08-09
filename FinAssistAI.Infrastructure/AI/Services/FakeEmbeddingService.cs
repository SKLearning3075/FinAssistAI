using FinAssistAI.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Services
{
    public class FakeEmbeddingService : IEmbeddingService
    {
        public Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
        {
            var random = new Random(text.GetHashCode());

            var embedding = new float[1536]; // Assuming the embedding size is 1536

            for (int i = 0; i < embedding.Length; i++)
            {
                embedding[i] = (float)random.NextDouble();
            }
            return Task.FromResult(embedding);
        }
    }
}
