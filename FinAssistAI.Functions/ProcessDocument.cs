using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FinAssistAI.Functions;

public class ProcessDocument
{
    private readonly ILogger<ProcessDocument> _logger;

    public ProcessDocument(ILogger<ProcessDocument> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ProcessDocument))]
    public async Task Run([BlobTrigger("finassist-document/{name}", Connection = "AzureWebJobsStorage")] Stream stream, string name)
    {
        using var blobStreamReader = new StreamReader(stream);
        var content = await blobStreamReader.ReadToEndAsync();
        _logger.LogInformation("C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}", name, content);
    }
}