using FinAssistAI.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace FinAssistAI.Infrastructure.AI.Services
{
    public class TextExtractionService : ITextExtractionService
    {
        public Task<string> ExtractTextAsync(string filePath, CancellationToken cancellationToken = default)
        {
           var extractedText = new StringBuilder();
           
            using (var document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    extractedText.Append(page.Text);
                }
            }
            return Task.FromResult(extractedText.ToString());
        }
    }
}
