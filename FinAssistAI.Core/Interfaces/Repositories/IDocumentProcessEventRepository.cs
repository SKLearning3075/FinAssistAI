using FinAssistAI.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Interfaces.Repositories
{
    public interface IDocumentProcessEventRepository
    {
        Task AddAsync(DocumentProcessEvent documentProcessEvent);
        Task<DocumentProcessEvent?> GetByIdAsync(Guid documentProcessEventId);
        Task UpdateAsync(DocumentProcessEvent documentProcessEvent);

        bool IsExist(string idempotencyKey);
    }
}
