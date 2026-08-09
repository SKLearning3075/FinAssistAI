using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Interfaces.Services
{
    public interface IEmbeddingService
    {
        Task<float[]> GenerateEmbeddingAsync(
        string text,
        CancellationToken cancellationToken = default);
    }
}
