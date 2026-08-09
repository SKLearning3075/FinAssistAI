using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Search.Services
{
    public class FakeSearchIndexService : ISearchIndexService
    {
        public Task IndexDocumentAsync(SearchDocument document, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("--------------------------------");

            Console.WriteLine($"Chunk : {document.ChunkNumber}");

            Console.WriteLine($"Department : {document.Department}");

            Console.WriteLine($"Country : {document.Country}");

            Console.WriteLine($"Embedding Size : {document.ContentVector.Length}");

            Console.WriteLine("--------------------------------");

            return Task.CompletedTask;
        }
    }
}
