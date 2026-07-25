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
        private readonly IConversationService _conversationService;
        public ChatService(IAIChatClient chatClient, IConversationService conversationService)
        {
            _chatClient = chatClient;
            _conversationService = conversationService;
        }

        public async Task<ChatResponse> AskAsync(ChatRequest request, CancellationToken cancellationToken = default)
        {
            // Step 1
            var conversation =
                await _conversationService.GetOrCreateConversationAsync(
                    request.ConversationId,
                    request.UserId);

            // Step 2
            await _conversationService.AddUserMessageAsync(
                conversation,
                request.Message);

            var aiRequest = new AIChatRequest
            {
                SystemPrompt = request.SystemPrompt,
                Messages = conversation.Messages
                                            .Select(x => new ChatMessage
                                            {
                                                Role = x.Role.ToString().ToLower(),
                                                Content = x.Content
                                            }).ToList()
            };

            var aiResponse =
            await _chatClient.GenerateResponseAsync(aiRequest, cancellationToken);

            // Step 5
            await _conversationService.AddAssistantMessageAsync(
                conversation,
                aiResponse.Content,
                aiResponse.PromptTokens,
                aiResponse.CompletionTokens,
                aiResponse.TotalTokens);

            // Step 6
            await _conversationService.SaveAsync(conversation);

            return new ChatResponse
            {
                ConversationId = conversation.Id,
                Answer = aiResponse.Content
            };
        }
    }
}
