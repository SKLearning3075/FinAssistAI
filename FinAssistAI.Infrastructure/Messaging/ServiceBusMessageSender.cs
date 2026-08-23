using Azure;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Infrastructure.AI.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Messaging
{
    public class ServiceBusMessageSender : IMessagePublisher
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string _queueName;
        private readonly ServiceBusSender _sender;

        public ServiceBusMessageSender(IOptions<AzureServiceBusOptions> serviceBusOptions)
        {
            var _option = serviceBusOptions.Value;

            _serviceBusClient = new ServiceBusClient(
                _option.FullyQualifiedNamespace,
                new DefaultAzureCredential(
                new DefaultAzureCredentialOptions
                {
                    TenantId = _option.TanantId
                })
                );

            _queueName = _option.QueueName;
            _sender = _serviceBusClient.CreateSender(_queueName);
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(message);

            var serviceBusMessage = new ServiceBusMessage(json)
            {
                ContentType = "application/json"
            };
            await _sender.SendMessageAsync(serviceBusMessage, cancellationToken);
        }
    }
}
