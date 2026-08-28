using Azure;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
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
using FinAssistAI.Infrastructure.Messaging;
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
            //services.AddSingleton<SearchIndexClient>(sp =>
            //{
            //    var options = sp
            //        .GetRequiredService<IOptions<AzureSearchOptions>>()
            //        .Value;

            //    return new SearchIndexClient(
            //        new Uri(options.Endpoint),
            //        new AzureKeyCredential(options.ApiKey));
            //});

            services.AddSingleton<ServiceBusClient>(sp =>
            {
                var options = sp
                    .GetRequiredService<IOptions<AzureServiceBusOptions>>()
                    .Value;

                var credential = new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions
                    {
                        TenantId = options.TanantId
                    });

                return new ServiceBusClient(
                    options.FullyQualifiedNamespace,
                    credential);
            });
            // Bind configuration
            services.Configure<OpenRouterOptions>(configuration.GetSection("OpenRouter"));
            services.Configure<AISettingsOptions>(configuration.GetSection("AISettings"));
            services.Configure<StorageOptions>(configuration.GetSection("Storage"));
            services.Configure<AzureSearchOptions>(configuration.GetSection("AzureSearch"));
            services.Configure<AzureOpenAIOptions>(configuration.GetSection("AzureOpenAI"));
            services.Configure<AzureBlobOptions>(configuration.GetSection("AzureBlob"));
            services.Configure<AzureServiceBusOptions>(configuration.GetSection("AzureServiceBus"));

            // Register DbContext
            services.AddDbContext<FinAssistDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),sqlOptions => sqlOptions.CommandTimeout(180));
            });

            // Register HttpClient + AI Client
            services.AddSingleton<IMessagePublisher,ServiceBusMessageSender>();
            //services.AddSingleton<ServiceBusClient>();

            // Register business service
            services.AddSingleton<IAIChatClient, AzureOpenAIChatClient>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IConversationService, ConversationService>();
            services.AddScoped<DocumentUploadOrchestrator>();
            services.AddSingleton<IDocumentProcessingQueue, InMemoryDocumentProcessingQueue>();
            services.AddHostedService<DocumentProcessingWorker>();
            services.AddScoped<IDocumentProcessor, DocumentProcessor>();
            services.AddScoped<ITextExtractionService, TextExtractionService>();
            services.AddScoped<IChunkingService, ChunkingService>();
            services.AddScoped<IEmbeddingService, AzureOpenAIEmbeddingService>();
            services.AddScoped<ISearchIndexService, AzureSearchIndexService>();
            services.AddScoped<IAzureSearchDocumentService, AzureSearchDocumentService>();
            services.AddScoped<IRagRetrievalService, RagRetrievalService>();
            services.AddScoped<IRagService,RagService>();
            services.AddScoped<IRagChatService, AzureOpenAIRagChatService>();
            services.AddScoped<IDocumentStorageService, AzureBlobDocumentStorageService>();
            
            


            //services.AddHttpClient<IAIChatClient, OpenRouterChatClient>();
            //services.AddHttpClient<IAIChatClient, AzureOpenAIChatClient>();
            //services.AddSingleton<AzureOpenAIService>();
            //services.AddScoped<IEmbeddingService, FakeEmbeddingService>();
            //services.AddScoped<IChatService,AzureOpenAIChatService>();
            services.AddSingleton<AzureSearchIndexManager>();
            //services.AddScoped<ISearchIndexService, FakeSearchIndexService>();
            //services.AddScoped<IDocumentStorageService, LocalDocumentStorageService>();

            // Register Repository service
            //services.AddScoped<IConversationRepository,
            //              InMemoryConversationRepository>();
            services.AddScoped<IConversationRepository,
                          EFConversationRepository>();
            services.AddScoped<IDocumentRepository,
                          DocumentRepository>();
            services.AddScoped<IDocumentProcessEventRepository, 
                          DocumentProcessEventRepository>();

            return services;
        }
    }
}
