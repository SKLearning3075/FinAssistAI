using FinAssistAI.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Interfaces.Services
{
    public interface IDocumentStorageService
    {
        public Task<StoredDocumentResponse> SaveAsync(
        Stream stream,
        string fileName,
        CancellationToken cancellationToken = default);
    }
}
