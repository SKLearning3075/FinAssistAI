using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using UglyToad.PdfPig.Tokens;

namespace FinAssistAI.Infrastructure.Queue
{
    public class InMemoryDocumentProcessingQueue : IDocumentProcessingQueue
    {
        private readonly Channel<DocumentProcessingMessage> _queue;
        public InMemoryDocumentProcessingQueue()
        {
            this._queue = Channel.CreateUnbounded<DocumentProcessingMessage>();
        }

        public async ValueTask<DocumentProcessingMessage> DequeueAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Queue Instance (Reader): {GetHashCode()}");

            var message = await _queue.Reader.ReadAsync(cancellationToken);

            Console.WriteLine($"Dequeued: {message.DocumentId}");

            return message;
        }

        public async ValueTask QueueAsync(DocumentProcessingMessage message)
        {
            Console.WriteLine($"Queue Instance (Writer): {GetHashCode()}");
            Console.WriteLine($"Queued: {message.DocumentId}");

            await _queue.Writer.WriteAsync(message);
        }
    }
}
