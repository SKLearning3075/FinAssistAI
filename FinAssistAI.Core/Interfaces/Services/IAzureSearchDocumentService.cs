using FinAssistAI.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Interfaces.Services
{
    public interface IAzureSearchDocumentService
    {
        Task<IReadOnlyList<SearchResult>> VectorSearchAsync(
        float[] queryVector,
        int topK = 5,
        CancellationToken cancellationToken = default);
    }
}
