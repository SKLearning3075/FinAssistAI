using FinAssistAI.Core.Models.Request;
using FinAssistAI.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Interfaces.Services
{
    public interface IChatService
    {
        Task<ChatResponse> AskAsync(ChatRequest request, CancellationToken cancellationToken = default);
    }
}
