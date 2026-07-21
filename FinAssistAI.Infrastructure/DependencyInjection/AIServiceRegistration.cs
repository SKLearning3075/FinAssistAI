using FinAssistAI.Core.Interfaces.AI;
using FinAssistAI.Core.Interfaces.Services;
using FinAssistAI.Core.Services;
using FinAssistAI.Infrastructure.AI.Clients;
using FinAssistAI.Infrastructure.AI.Configuration;
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

            // Register HttpClient + AI Client
            services.AddHttpClient<IAIChatClient, OpenRouterChatClient>();

            // Register business service
            services.AddScoped<IChatService, ChatService>();

            return services;
        }
    }
}
