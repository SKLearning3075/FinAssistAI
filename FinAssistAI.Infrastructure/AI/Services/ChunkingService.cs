using FinAssistAI.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Services
{
    public class ChunkingService : IChunkingService
    {
        public Task<IReadOnlyList<string>> ChunkAsync(string text, CancellationToken cancellationToken = default)
        {
            int chunkSize = 1000;
            int overlapChunk = 200;
            int extractedTextLength = 0;
            List<string> lstChunk = new List<string>();

            if (string.IsNullOrEmpty(text))
            {
                return Task.FromResult<IReadOnlyList<string>>(lstChunk);
            }
            else
            {
                extractedTextLength = text.Length;
            }

           
            for (int i = 0; i <= extractedTextLength; i = (i + (chunkSize - overlapChunk)))
            {
                cancellationToken.ThrowIfCancellationRequested();

                var length = Math.Min(chunkSize, extractedTextLength - i);
                var Chunk = text.Substring(i, length);

                lstChunk.Add(Chunk);
            }

            return Task.FromResult<IReadOnlyList<string>>(lstChunk);
        }
    }
}
