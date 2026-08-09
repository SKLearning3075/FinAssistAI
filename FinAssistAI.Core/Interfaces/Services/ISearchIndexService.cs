using FinAssistAI.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Core.Interfaces.Services
{
    public interface ISearchIndexService
    {
        public Task IndexDocumentAsync(
        SearchDocument document,
        CancellationToken cancellationToken = default);
    }
}
