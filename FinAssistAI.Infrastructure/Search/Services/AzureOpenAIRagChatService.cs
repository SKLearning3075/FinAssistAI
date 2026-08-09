using Azure;
using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Infrastructure.AI.Configuration;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Search.Services
{
    public class AzureOpenAIRagChatService : IRagChatService
    {
        private readonly AzureOpenAIClient _azureOpenAIClient;
        private readonly string _chatDeploymentName;
        public AzureOpenAIRagChatService(IOptions<AzureOpenAIOptions> options)
        {
            var azureOpenAIOptions = options.Value;

            _azureOpenAIClient = new AzureOpenAIClient(
                new Uri(azureOpenAIOptions.Endpoint),
                new AzureKeyCredential(azureOpenAIOptions.ApiKey));

            _chatDeploymentName = azureOpenAIOptions.ChatDeployment;
        }
        public async Task<string> GenerateAnswerAsync(string prompt, CancellationToken cancellationToken = default)
        {
            var chatClient = _azureOpenAIClient.GetChatClient(_chatDeploymentName);

            var messages = new List<ChatMessage>
        {
            new SystemChatMessage(
                """
                You are a financial assistant.

                Answer the user's question using only the
                information provided in the context.

                If the answer cannot be found in the provided
                context, say that the information is not
                available in the provided documents.

                Do not make up information.
                """),

            new UserChatMessage(prompt)
        };
            //ChatCompletionOptions options = new()
            //{
            //    Temperature = 0.7f,
            //    MaxOutputTokenCount = 500
            //};
            ChatCompletionOptions options = new();

            #pragma warning disable AOAI001
            //options.SetNewMaxCompletionTokensPropertyEnabled();
            #pragma warning restore AOAI001

            ChatCompletion chatCompletion = await chatClient.CompleteChatAsync(
                messages,
                options,
                cancellationToken);

            return chatCompletion.Content[0].Text;
        }
    }
}
