using Azure;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.Exporter;
using Azure.Search.Documents;
using FinAssistAI.Core.Interfaces.Repositories;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Infrastructure.AI.Configuration;
using FinAssistAI.Infrastructure.AI.Services;
using FinAssistAI.Infrastructure.DependencyInjection;
using FinAssistAI.Infrastructure.Persistence;
using FinAssistAI.Infrastructure.Repositories;
using FinAssistAI.Infrastructure.Search.Services;
using FinAssistAI.ServiceBusFunction.Contracts;
using FinAssistAI.ServiceBusFunction.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenTelemetry;

var builder = FunctionsApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;


var keyVaultUri = builder.Configuration["KeyVault:VaultUri"];

var credential = new DefaultAzureCredential(
    new DefaultAzureCredentialOptions
    {
        TenantId = "17018ab6-95a3-43d1-83da-2438b7821432"
    });

if (!string.IsNullOrWhiteSpace(keyVaultUri))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUri),
        credential);
}

// Add services to the container.
builder.Services.AddSingleton<SearchClient>(sp =>
{
    var options = sp
        .GetRequiredService<IOptions<AzureSearchOptions>>()
        .Value;

    return new SearchClient(
        new Uri(options.Endpoint),
        options.IndexName,
        new AzureKeyCredential(options.ApiKey));
});


builder.ConfigureFunctionsWebApplication();

if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING")))
{
    builder.Services.AddOpenTelemetry()
        .UseFunctionsWorkerDefaults()
        .UseAzureMonitorExporter();
}

// Register DbContext
builder.Services.AddDbContext<FinAssistDbContext>(options =>
{
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"), sqlOptions => sqlOptions.CommandTimeout(180));
});

// Bind configuration
builder.Services.Configure<OpenRouterOptions>(configuration.GetSection("OpenRouter"));
builder.Services.Configure<AISettingsOptions>(configuration.GetSection("AISettings"));
builder.Services.Configure<StorageOptions>(configuration.GetSection("Storage"));
builder.Services.Configure<AzureSearchOptions>(configuration.GetSection("AzureSearch"));
builder.Services.Configure<AzureOpenAIOptions>(configuration.GetSection("AzureOpenAI"));
builder.Services.Configure<AzureBlobOptions>(configuration.GetSection("AzureBlob"));
builder.Services.Configure<AzureServiceBusOptions>(configuration.GetSection("AzureServiceBus"));

builder.Services.AddScoped<IServiceBusProcessDocument, ServiceBusProcessDocumentService>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IDocumentProcessEventRepository, DocumentProcessEventRepository>();
builder.Services.AddScoped<ITextExtractionService, TextExtractionService>();
builder.Services.AddScoped<IChunkingService, ChunkingService>();
builder.Services.AddScoped<IEmbeddingService, AzureOpenAIEmbeddingService>();
builder.Services.AddScoped<ISearchIndexService, AzureSearchIndexService>();
builder.Services.AddSingleton<AzureSearchIndexManager>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var indexManager =
        scope.ServiceProvider
            .GetRequiredService<AzureSearchIndexManager>();

    await indexManager.CreateIndexAsync();
}

app.Run();