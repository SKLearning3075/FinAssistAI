using FinAssistAI.Core.Interfaces.AI;
using FinAssistAI.Core.Models.Common;
using FinAssistAI.Core.Models.Request;
using FinAssistAI.Infrastructure.AI.Configuration;
using FinAssistAI.Infrastructure.AI.Models;
using Microsoft.Extensions.Options;
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
        private readonly OpenRouterOptions _options;
        public OpenRouterChatClient(HttpClient httpClient, IOptions<OpenRouterOptions> options)
        {
            this._httpClient = httpClient;
            this._options = options.Value;
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

            return BuildResult(openRouterResponse);

        }

        private OpenRouterRequest BuildRequest(AIChatRequest request)
        {
            return new OpenRouterRequest
            {
                Model = _options.ChatModel,

                Messages =
                [
                    new ChatMessage
                {
                    Role = "user",
                    Content = request.Messages.FirstOrDefault()?.Content ?? string.Empty
                }
                ]
            };
        }

        private void ConfigureHttpClient()
        {
            _httpClient.BaseAddress =
                new Uri(_options.BaseUrl);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _options.ApiKey);

            _httpClient.DefaultRequestHeaders.Accept.Clear();

            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static HttpRequestMessage CreateHttpRequest(
            OpenRouterRequest request)
        {
            return new HttpRequestMessage(
                HttpMethod.Post,
                "chat/completions")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(request),
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
