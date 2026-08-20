using Azure;
using Azure.AI.OpenAI;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Request;
using FinAssistAI.Core.Models.Response;
using FinAssistAI.Infrastructure.AI.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Search.Services
{
    public class AzureOpenAIChatService: IChatService
    {
        private readonly AzureOpenAIClient _client;
        private readonly string _deploymentName;

        public AzureOpenAIChatService(
            IOptions<AzureOpenAIOptions> options)
        {
            var settings = options.Value;

            _client = new AzureOpenAIClient(
                new Uri(settings.Endpoint),
                new AzureKeyCredential(settings.ApiKey));

            _deploymentName = settings.ChatDeployment;
        }

        public async Task<ChatResponse> AskAsync(ChatRequest request, CancellationToken cancellationToken = default)
        {
           var _chatClient =  _client.GetChatClient(_deploymentName);
           //return chatClient.CompleteChatAsync(request, cancellationToken);

            ChatMessage[] messages = new ChatMessage[]
            {
                new UserChatMessage(request.Message),
                new SystemChatMessage(request.SystemPrompt ?? "You are a helpful assistant."),


            };

            var response = await _chatClient.CompleteChatAsync(messages);


            return response.Value is not null ? new ChatResponse
            {
                ConversationId = request.ConversationId ?? Guid.NewGuid(),
                //Answer = response.Value.Choices.FirstOrDefault()?.Message.Content ?? string.Empty
            } : new ChatResponse
            {
                ConversationId = request.ConversationId ?? Guid.NewGuid(),
                Answer = "No response from the AI model."
            };
        }
    }
}
