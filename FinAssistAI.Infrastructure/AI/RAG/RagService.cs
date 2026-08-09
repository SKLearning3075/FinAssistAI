using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Response;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.AI.RAG
{
    public class RagService : IRagService
    {
        private readonly IRagRetrievalService _retrievalService;
        private readonly IChatService _chatService;
        private readonly IRagChatService _ragChatService;

        public RagService(
            IRagRetrievalService retrievalService,
            IChatService chatService,
            IRagChatService ragChatService)
        {
            _retrievalService = retrievalService;
            _chatService = chatService;
            _ragChatService = ragChatService;
        }
        public async Task<RagResponse> AskAsync(string question, CancellationToken cancellationToken = default)
        {
            // 1. Retrieve relevant chunks
            var chunks =
                await _retrievalService.RetrieveAsync(
                    question,
                    5,
                    cancellationToken);

            // 2. Build context
            var context = BuildContext(chunks);

            // 3.Build prompt
        var prompt = BuildPrompt(
            question,
            context);

            // 4. Ask Azure OpenAI
            var answer =
                await _ragChatService.GenerateAnswerAsync(
                    prompt,
                    cancellationToken);

            // 5. Return answer + sources
            return new RagResponse
            {
                Question = question,
                Answer = answer,
                Sources = chunks
            };

        }

        private static string BuildContext(
        IReadOnlyList<SearchResult> chunks)
        {
            var builder = new StringBuilder();

            foreach (var chunk in chunks)
            {
                builder.AppendLine(
                    $"Source: {chunk.DocumentId}");

                builder.AppendLine(
                    chunk.Content);

                builder.AppendLine();
            }

            return builder.ToString();
        }

        private static string BuildPrompt(
        string question,
        string context)
        {
            return $"""
            Answer the user's question using only the
            information provided in the context below.

            If the context does not contain enough information
            to answer the question, clearly say that the answer
            is not available in the provided documents.

            Context:
            {context}

            User Question:
            {question}

            Provide a clear and concise answer.
            """;
        }

        public Task<string> GenerateAnswerAsync(string prompt, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
