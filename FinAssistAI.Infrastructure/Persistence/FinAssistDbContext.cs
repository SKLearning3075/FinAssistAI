using FinAssistAI.Infrastructure.Entities;
using FinAssistAI.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Persistence
{
    public class FinAssistDbContext:DbContext
    {
        public FinAssistDbContext(DbContextOptions<FinAssistDbContext> options) : base(options)
        {
        }

        public DbSet<ConversationEntity> Conversations { get; set; }
        public DbSet<ConversationMessageEntity> ConversationMessages { get; set; }
        public DbSet<DocumentEntity> Documents { get; set; }
        public DbSet<ProcessedMessage> ProcessedMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinAssistDbContext).Assembly);
        }
    }
}
