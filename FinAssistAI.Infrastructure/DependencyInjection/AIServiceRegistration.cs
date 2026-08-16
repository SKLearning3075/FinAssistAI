using Azure;
using Azure.Search.Documents.Indexes;
using FinAssistAI.Core.Interfaces.AI;
using FinAssistAI.Core.Interfaces.Repositories;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Services;
using FinAssistAI.Infrastructure.AI.Clients;
using FinAssistAI.Infrastructure.AI.Configuration;
using FinAssistAI.Infrastructure.AI.RAG;
using FinAssistAI.Infrastructure.AI.Services;
using FinAssistAI.Infrastructure.Background;
using FinAssistAI.Infrastructure.Persistence;
using FinAssistAI.Infrastructure.Processing;
using FinAssistAI.Infrastructure.Queue;
using FinAssistAI.Infrastructure.Repositories;
using FinAssistAI.Infrastructure.Search;
using FinAssistAI.Infrastructure.Search.Services;
using FinAssistAI.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FinAssistAI.Infrastructure.DependencyInjection
{
    public static class AIServiceRegistration
    {
        public static IServiceCollection RegisterAIService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<SearchIndexClient>(sp =>
            {
                var options = sp
                    .GetRequiredService<IOptions<AzureSearchOptions>>()
                    .Value;

                return new SearchIndexClient(
                    new Uri(options.Endpoint),
                    new AzureKeyCredential(options.ApiKey));
            });

            // Bind configuration
            services.Configure<OpenRouterOptions>(configuration.GetSection("OpenRouter"));
            services.Configure<AISettingsOptions>(configuration.GetSection("AISettings"));
            services.Configure<StorageOptions>(configuration.GetSection("Storage"));
            services.Configure<AzureSearchOptions>(configuration.GetSection("AzureSearch"));
            services.Configure<AzureOpenAIOptions>(configuration.GetSection("AzureOpenAI"));
            services.Configure<AzureBlobOptions>(configuration.GetSection("AzureBlob"));

            // Register DbContext
            services.AddDbContext<FinAssistDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),sqlOptions => sqlOptions.CommandTimeout(180));
            });

            // Register HttpClient + AI Client
            services.AddHttpClient<IAIChatClient, OpenRouterChatClient>();

            // Register business service
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IConversationService, ConversationService>();
            //services.AddScoped<IDocumentStorageService, LocalDocumentStorageService>();
            services.AddScoped<DocumentUploadOrchestrator>();
            services.AddSingleton<IDocumentProcessingQueue, InMemoryDocumentProcessingQueue>();
            services.AddHostedService<DocumentProcessingWorker>();
            services.AddScoped<IDocumentProcessor, DocumentProcessor>();
            services.AddScoped<ITextExtractionService, TextExtractionService>();
            services.AddScoped<IChunkingService, ChunkingService>();
            //services.AddScoped<IEmbeddingService, FakeEmbeddingService>();
            services.AddScoped<IEmbeddingService, AzureOpenAIEmbeddingService>();
            //services.AddScoped<ISearchIndexService, FakeSearchIndexService>();
            services.AddScoped<ISearchIndexService, AzureSearchIndexService>();
            //services.AddSingleton<AzureSearchIndexManager>();
            services.AddScoped<IAzureSearchDocumentService, AzureSearchDocumentService>();
            services.AddScoped<IRagRetrievalService, RagRetrievalService>();
            //services.AddScoped<IChatService,AzureOpenAIChatService>();
            services.AddScoped<IRagService,RagService>();
            services.AddScoped<IRagChatService, AzureOpenAIRagChatService>();
            services.AddScoped<IDocumentStorageService, AzureBlobDocumentStorageService>();



            // Register Repository service
            //services.AddScoped<IConversationRepository,
            //              InMemoryConversationRepository>();
            services.AddScoped<IConversationRepository,
                          EFConversationRepository>();
            services.AddScoped<IDocumentRepository,
                          DocumentRepository>();


            return services;
        }
    }
}
