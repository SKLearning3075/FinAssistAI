using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Interfaces.Services
{
    public interface IRagChatService
    {
        Task<string> GenerateAnswerAsync(
       string prompt,
       CancellationToken cancellationToken = default);
    }
}
