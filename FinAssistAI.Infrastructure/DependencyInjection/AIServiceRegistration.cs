using FinAssistAI.Core.Interfaces.AI;
using FinAssistAI.Core.Interfaces.Repositories;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Services;
using FinAssistAI.Infrastructure.AI.Clients;
using FinAssistAI.Infrastructure.AI.Configuration;
using FinAssistAI.Infrastructure.AI.Services;
using FinAssistAI.Infrastructure.Background;
using FinAssistAI.Infrastructure.Persistence;
using FinAssistAI.Infrastructure.Processing;
using FinAssistAI.Infrastructure.Queue;
using FinAssistAI.Infrastructure.Repositories;
using FinAssistAI.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinAssistAI.Infrastructure.DependencyInjection
{
    public static class AIServiceRegistration
    {
        public static IServiceCollection RegisterAIService(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind configuration
            services.Configure<OpenRouterOptions>(configuration.GetSection("OpenRouter"));
            services.Configure<AISettingsOptions>(configuration.GetSection("AISettings"));
            services.Configure<StorageOptions>(configuration.GetSection("Storage"));

            // Register DbContext
            services.AddDbContext<FinAssistDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            // Register HttpClient + AI Client
            services.AddHttpClient<IAIChatClient, OpenRouterChatClient>();

            // Register business service
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IConversationService, ConversationService>();
            services.AddScoped<IDocumentStorageService, LocalDocumentStorageService>();
            services.AddScoped<DocumentUploadOrchestrator>();
            services.AddSingleton<IDocumentProcessingQueue, InMemoryDocumentProcessingQueue>();
            services.AddHostedService<DocumentProcessingWorker>();
            services.AddScoped<IDocumentProcessor, DocumentProcessor>();
            services.AddScoped<ITextExtractionService, TextExtractionService>();
            services.AddScoped<IChunkingService, ChunkingService>();


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
