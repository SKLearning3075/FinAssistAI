using Azure.Messaging.ServiceBus;
using FinAssistAI.Contracts.Events;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Infrastructure.Processing;
using FinAssistAI.ServiceBusFunction.Contracts;
using FinAssistAI.ServiceBusFunction.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinAssistAI.ServiceBusFunction;

public class ProcessDocumentUploadedFunction
{
    private readonly ILogger<ProcessDocumentUploadedFunction> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IServiceBusProcessDocument _serviceBusProcessDocument;

    public ProcessDocumentUploadedFunction(ILogger<ProcessDocumentUploadedFunction> logger
        , IServiceScopeFactory serviceScopeFactory
        , IServiceBusProcessDocument serviceBusProcessDocument
            )
    {
        _logger = logger;
        _scopeFactory = serviceScopeFactory;
        _serviceBusProcessDocument = serviceBusProcessDocument;
    }

    [Function("ProcessDocumentUploaded")]
    public async Task Run(
        [ServiceBusTrigger("finassistai-sb-queue", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
       
        var messageBody = message.Body.ToString();
        var documentUploadedEvent =
            JsonSerializer.Deserialize<DocumentUploadedEvent>(messageBody);

        if (documentUploadedEvent == null)
        {
            throw new InvalidOperationException(
                "Unable to deserialize DocumentUploadedEvent.");
        }

        await _serviceBusProcessDocument.ProcessDocumentAsync(documentUploadedEvent.DocumentId,
            documentUploadedEvent.EventId,
            documentUploadedEvent.IdempotencyKey, CancellationToken.None);

        await messageActions.CompleteMessageAsync(message);
    }
}