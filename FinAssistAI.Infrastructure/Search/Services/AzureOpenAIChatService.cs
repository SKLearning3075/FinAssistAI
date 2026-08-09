using Azure;
using Azure.AI.OpenAI;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Request;
using FinAssistAI.Core.Models.Response;
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

        public Task<ChatResponse> AskAsync(ChatRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
