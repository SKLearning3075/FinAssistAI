using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Response;
using FinAssistAI.Infrastructure.AI.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Storage
{
    public class LocalDocumentStorageService : IDocumentStorageService
    {
        private readonly StorageOptions _storageOptions;
        public LocalDocumentStorageService(IOptions<StorageOptions> options)
        {
            this._storageOptions = options.Value;
        }
        public async Task<StoredDocumentResponse> SaveAsync(Stream stream, string fileName, CancellationToken cancellationToken = default)
        {
            var uploadFolder = _storageOptions.UploadFolder;
            var filePath = Path.Combine(uploadFolder, fileName);

            // Validate configuration
            if (string.IsNullOrWhiteSpace(_storageOptions.UploadFolder))
                throw new InvalidOperationException("Upload folder is not configured.");

            // Create directory if it doesn't exist
            if (!Directory.Exists(_storageOptions.UploadFolder))
            {
                Directory.CreateDirectory(_storageOptions.UploadFolder);
            }

            // Generate a unique file name
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

            var uniqueFilePath = Path.Combine(uploadFolder, uniqueFileName);

            await using (var fileStream = new FileStream(uniqueFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await stream.CopyToAsync(fileStream, cancellationToken);
            }

            StoredDocumentResponse storedDocumentResponse = new StoredDocumentResponse
            {
                FileName = uniqueFileName,
                FilePath = uniqueFilePath,
                Size = new FileInfo(uniqueFilePath).Length,
                UploadedOn = DateTime.UtcNow
            };
            return storedDocumentResponse;
        }
    }
}
