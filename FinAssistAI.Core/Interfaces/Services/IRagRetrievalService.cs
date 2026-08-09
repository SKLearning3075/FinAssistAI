using FinAssistAI.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Interfaces.Services
{
    public interface IRagRetrievalService
    {
        Task<IReadOnlyList<SearchResult>> RetrieveAsync(
        string question,
        int topK = 5,
        CancellationToken cancellationToken = default);
    }
}
