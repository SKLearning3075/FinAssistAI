using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Interfaces.Services
{
    public interface IChunkingService
    {
        Task<IReadOnlyList<string>> ChunkAsync(
        string text,
        CancellationToken cancellationToken = default);
    }
}
