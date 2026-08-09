using Azure.Storage.Blobs;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Models.Response;
using FinAssistAI.Infrastructure.AI.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Storage
{
    public class AzureBlobDocumentStorageService : IDocumentStorageService
    {
        private readonly BlobContainerClient _blobContainerClient;
        public AzureBlobDocumentStorageService(IOptions<AzureBlobOptions> options) { 
            var configuration = options.Value;

            if (string.IsNullOrWhiteSpace(configuration.ConnectionString))
            {
                throw new ArgumentException(
                    "Azure Blob Storage connection string is not configured.");
            }

            if (string.IsNullOrWhiteSpace(configuration.ContainerName))
            {
                throw new ArgumentException(
                    "Azure Blob Storage container name is not configured.");
            }

            var blobServiceClient =
            new BlobServiceClient(configuration.ConnectionString);

            _blobContainerClient =
            blobServiceClient.GetBlobContainerClient(
                configuration.ContainerName);
        }
        public async Task<StoredDocumentResponse> SaveAsync(Stream content, string fileName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(
                    "File name cannot be empty.",
                    nameof(fileName));
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            // Generate a unique file name
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

            var blobContainerResponse = await _blobContainerClient.CreateIfNotExistsAsync(
            cancellationToken: cancellationToken);

            BlobClient blobClient =
            _blobContainerClient.GetBlobClient(uniqueFileName);

           var blobContentResponse = await blobClient.UploadAsync(
           content,
           overwrite: true,
           cancellationToken);

            // Get uploaded blob information
            var properties =
                await blobClient.GetPropertiesAsync(
                    cancellationToken: cancellationToken);

            return new StoredDocumentResponse
            {
                FileName = uniqueFileName,

                // Azure doesn't have a local physical path.
                // Store the blob URI here.
                FilePath = blobClient.Uri.ToString(),

                Size = properties.Value.ContentLength,

                UploadedOn = DateTime.UtcNow
            };
        }
    }
}
