using FinAssistAI.Core.Enums;
using FinAssistAI.Core.Interfaces.AI;
using FinAssistAI.Core.Models.Common;
using FinAssistAI.Core.Models.Request;
using FinAssistAI.Infrastructure.AI.Configuration;
using FinAssistAI.Infrastructure.AI.Models;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.Clients
{
    public class OpenRouterChatClient : IAIChatClient
    {
        private readonly HttpClient _httpClient;
        private readonly OpenRouterOptions _openRouterOptions;
        private readonly AISettingsOptions _aiSettingsOptions;
        public OpenRouterChatClient(HttpClient httpClient, IOptions<OpenRouterOptions> openRouterOptions, IOptions<AISettingsOptions> aiSettingsOptions)
        {
            this._httpClient = httpClient;
            this._openRouterOptions = openRouterOptions.Value;
            this._aiSettingsOptions = aiSettingsOptions.Value;
        }

        public async Task<AIChatResult> GenerateResponseAsync(AIChatRequest request, CancellationToken cancellationToken = default)
        {
            var openRouterRequest = BuildRequest(request);
            ConfigureHttpClient();

            var httpRequest = CreateHttpRequest(openRouterRequest);

            var response = await _httpClient.SendAsync(
            httpRequest,
            cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            //response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Status: {(int)response.StatusCode}\n" +
                    $"Reason: {response.ReasonPhrase}\n" +
                    $"Body:\n{responseBody}");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var openRouterResponse =
                JsonSerializer.Deserialize<OpenRouterResponse>(
                    responseBody,
                    options);

            if (openRouterResponse == null)
                throw new Exception("Unable to parse OpenRouter response.");

            Console.WriteLine(openRouterResponse);
            return BuildResult(openRouterResponse);

        }
        private OpenRouterRequest BuildRequest(AIChatRequest request)
        {

            var messages = new List<ChatMessage>();

            if (!string.IsNullOrEmpty(request.SystemPrompt))
            {
                messages.Add(new ChatMessage
                {
                    Role = MessageRole.System.ToString().ToLower(),
                    Content = request.SystemPrompt
                });
            }

            messages.AddRange(
                    request.Messages.Select(m => new ChatMessage
                    {
                        Role = m.Role,
                        Content = m.Content
                    }));
            Console.WriteLine($"User message added to conversation {request.Messages.Count}");
            var openRouterRequest = new OpenRouterRequest
            {
                Model = _openRouterOptions.ChatModel,
                Temperature = _aiSettingsOptions.Temperature,
                MaxTokens = _aiSettingsOptions.MaxTokens,
                Messages = messages
            }; 

            var json = JsonSerializer.Serialize(openRouterRequest, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            Console.WriteLine(json);

            return openRouterRequest;
        }
        private void ConfigureHttpClient()
        {
            _httpClient.BaseAddress =
                new Uri(_openRouterOptions.BaseUrl);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _openRouterOptions.ApiKey);

            _httpClient.DefaultRequestHeaders.Accept.Clear();

            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private static HttpRequestMessage CreateHttpRequest(
            OpenRouterRequest request)
        {
            return new HttpRequestMessage(HttpMethod.Post,"chat/completions")
            {
                    Content = new StringContent(JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json")
            };
        }
        private static AIChatResult BuildResult(
            OpenRouterResponse response)
        {
            return new AIChatResult
            {
                Content =
                    response.Choices.FirstOrDefault()?.Message?.Content
                    ?? string.Empty,

                Model = response.Model,

                PromptTokens =
                    response.Usage?.PromptTokens ?? 0,

                CompletionTokens =
                    response.Usage?.CompletionTokens ?? 0
            };
        }
    }

}
