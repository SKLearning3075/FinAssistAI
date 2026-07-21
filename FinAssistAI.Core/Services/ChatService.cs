using FinAssistAI.Core.Interfaces.AI;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Common;
using FinAssistAI.Core.Models.Request;
using FinAssistAI.Core.Models.Response;

namespace FinAssistAI.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IAIChatClient _chatClient;
        public ChatService(IAIChatClient chatClient)
        {
            _chatClient = chatClient;
        }

        public async Task<ChatResponse> AskAsync(ChatRequest request, CancellationToken cancellationToken = default)
        {
            var aiRequest = new AIChatRequest
            {
                Messages =
                [
                    new ChatMessage
                    {
                        Role = "user",
                        Content = request.Message
                    }
                ]
            };
        
            var aiResult =
            await _chatClient.GenerateResponseAsync(aiRequest, cancellationToken);

            return new ChatResponse
            {
                Answer = aiResult.Content
            };
        }
    }
}
