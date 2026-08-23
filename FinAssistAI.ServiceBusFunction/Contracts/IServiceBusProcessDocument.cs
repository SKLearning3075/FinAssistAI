using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.ServiceBusFunction.Contracts
{
    public interface IServiceBusProcessDocument
    {
        Task ProcessDocumentAsync(Guid documentId, Guid eventId, string idempotencyKey, CancellationToken cancellationToken);
    }
}
