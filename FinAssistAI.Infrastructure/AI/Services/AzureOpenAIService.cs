using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Services
{
    public class AzureOpenAIService
    {
        private readonly ChatClient _chatClient;

        public AzureOpenAIService(IConfiguration configuration)
        {
            var endpoint = configuration["AzureOpenAI:Endpoint"];
            var deployment = configuration["AzureOpenAI:ChatDeployment"];

            var client = new AzureOpenAIClient(
                new Uri(endpoint!),
                new DefaultAzureCredential());

            _chatClient = client.GetChatClient(deployment);
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var response = await _chatClient.CompleteChatAsync(
            [
                new UserChatMessage(prompt)
            ]);

            return response.Value.Content[0].Text;
        }
    }
}
