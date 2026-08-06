using FinAssistAI.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Background
{
    public class DocumentProcessingWorker: BackgroundService
    {
        private readonly IDocumentProcessingQueue _queue;
        private readonly ILogger<DocumentProcessingWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DocumentProcessingWorker(
            IDocumentProcessingQueue queue,
            ILogger<DocumentProcessingWorker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _queue = queue;
            _logger = logger;
            _scopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Document Worker Started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var message =
                        await _queue.DequeueAsync(stoppingToken);

                    // Create a new DI scope
                    using var scope = _scopeFactory.CreateScope();

                    // Resolve scoped services
                    var processor = scope.ServiceProvider
                        .GetRequiredService<IDocumentProcessor>();

                    // Process document
                    await processor.ProcessAsync(
                        message.DocumentId,
                        stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation(
                        "Worker stopping gracefully.");

                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error processing document.");

                    // Continue processing next messages
                }
            }
        }
    }
}
