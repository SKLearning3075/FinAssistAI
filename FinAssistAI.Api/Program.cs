using Azure;
using Azure.Search.Documents;
using FinAssistAI.Infrastructure.AI.Configuration;
using FinAssistAI.Infrastructure.DependencyInjection;
using FinAssistAI.Infrastructure.Search.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.RegisterAIService(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var indexManager =
        scope.ServiceProvider
            .GetRequiredService<AzureSearchIndexManager>();

    await indexManager.CreateIndexAsync();
}

app.Run();
