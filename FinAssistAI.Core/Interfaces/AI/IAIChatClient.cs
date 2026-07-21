using FinAssistAI.Core.Models.Request;
using FinAssistAI.Infrastructure.AI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Interfaces.AI
{
    public interface IAIChatClient
    {
        Task<AIChatResult> GenerateResponseAsync(AIChatRequest request, CancellationToken cancellationToken = default);
    }
}
