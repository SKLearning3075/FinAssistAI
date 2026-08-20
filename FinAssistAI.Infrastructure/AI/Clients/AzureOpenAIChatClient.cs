using Azure.AI.OpenAI;
using Azure.Identity;
using FinAssistAI.Core.Interfaces.AI;
using FinAssistAI.Core.Models.Request;
using FinAssistAI.Infrastructure.AI.Configuration;
using FinAssistAI.Infrastructure.AI.Models;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Clients
{
    public class AzureOpenAIChatClient : IAIChatClient
    {
        private readonly ChatClient _chatClient;
        private readonly AISettingsOptions _aiSettingsOptions;
        private readonly AzureOpenAIOptions _azureOpenAIOptions;

        public AzureOpenAIChatClient(
            IOptions<AzureOpenAIOptions> azureOpenAIOptions,
            IOptions<AISettingsOptions> aiSettingsOptions)
        {
            _azureOpenAIOptions = azureOpenAIOptions.Value;

            var credential = new DefaultAzureCredential(
            new DefaultAzureCredentialOptions
            {
                TenantId = "17018ab6-95a3-43d1-83da-2438b7821432"
            });

            var azureOpenAIClient = new AzureOpenAIClient(
                new Uri(_azureOpenAIOptions.Endpoint),
                credential);

            _chatClient = azureOpenAIClient.GetChatClient(
                _azureOpenAIOptions.ChatDeployment);
            _aiSettingsOptions = aiSettingsOptions.Value;
        }
        public async Task<AIChatResult> GenerateResponseAsync(
               AIChatRequest request,
               CancellationToken cancellationToken = default)
        {
            var messages = new List<ChatMessage>();

            if (!string.IsNullOrWhiteSpace(request.SystemPrompt))
            {
                messages.Add(
                    new SystemChatMessage(request.SystemPrompt));
            }

            foreach (var message in request.Messages)
            {
                if (message.Role.Equals("user", StringComparison.OrdinalIgnoreCase))
                {
                    messages.Add(new UserChatMessage(message.Content));
                }
                else if (message.Role.Equals("assistant", StringComparison.OrdinalIgnoreCase))
                {
                    messages.Add(new AssistantChatMessage(message.Content));
                }
            }
            var options = new ChatCompletionOptions
            {

                //Temperature = (float)_aiSettingsOptions.Temperature,
                //MaxOutputTokenCount = _aiSettingsOptions.MaxTokens
            };

            var response = await _chatClient.CompleteChatAsync(
                messages,
                options,
                cancellationToken);

            var completion = response.Value;

            return new AIChatResult
            {
                Content = completion.Content.FirstOrDefault()?.Text
                          ?? string.Empty,

                Model = _azureOpenAIOptions.ChatDeployment,

                PromptTokens = completion.Usage?.InputTokenCount ?? 0,

                CompletionTokens = completion.Usage?.OutputTokenCount ?? 0
            };
        }
    }
}
