using FinAssistAI.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinAssistAI.Infrastructure.Persistence.Configurations
{
    public class ProcessedMessageConfiguration
    : IEntityTypeConfiguration<ProcessedMessage>
    {
        public void Configure(EntityTypeBuilder<ProcessedMessage> builder)
        {
            builder.ToTable("ProcessedMessages");

            builder.HasKey(x => x.IdempotencyKey);

            builder.Property(x => x.IdempotencyKey)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.EventId)
                   .IsRequired();

            builder.Property(x => x.ProcessedAt)
                   .IsRequired();
        }
    }
}
