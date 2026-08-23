using Azure.Identity;
using Azure.Storage.Blobs;
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
        public async Task<string> ExtractTextAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("filePath must not be null or empty.", nameof(filePath));

            cancellationToken.ThrowIfCancellationRequested();

            var extractedText = new StringBuilder();

            // Case 1: Local file exists
            //if (System.IO.File.Exists(filePath))
            //{
            //    using (var document = PdfDocument.Open(filePath))
            //    {
            //        foreach (var page in document.GetPages())
            //        {
            //            cancellationToken.ThrowIfCancellationRequested();
            //            extractedText.Append(page.Text);
            //        }
            //    }

            //    return extractedText.ToString();
            //}

            // Case 2: Try to treat as URI (Blob URI or HTTP/HTTPS)
            if (Uri.TryCreate(filePath, UriKind.Absolute, out var uri))
            {
                // Use Azure.Storage.Blobs.BlobClient with the provided URI.
                // This supports SAS tokens embedded in the URI or public blobs.

                var credential = new DefaultAzureCredential(
                new DefaultAzureCredentialOptions
                {
                    TenantId = "17018ab6-95a3-43d1-83da-2438b7821432"
                });

                var blobServiceClient = new BlobClient(
                    new Uri(filePath),
                    credential);


                // Download the blob as a stream (stream will be disposed after use)
                var downloadResponse = await blobServiceClient.DownloadStreamingAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

                await using (var stream = downloadResponse.Value.Content)
                {
                    using (var document = PdfDocument.Open(stream))
                    {
                        foreach (var page in document.GetPages())
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            extractedText.Append(page.Text);
                        }
                    }
                }

                return extractedText.ToString();
            }

            throw new ArgumentException("The provided filePath does not point to an existing local file or a valid absolute URI.", nameof(filePath));
        }
    }
}
