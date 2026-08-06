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
    public class ConversationMessageConfiguration : IEntityTypeConfiguration<ConversationMessageEntity>
    {
        public void Configure(EntityTypeBuilder<ConversationMessageEntity> builder)
        {
            builder.ToTable("ConversationMessages");

            builder.HasKey(x => x.MessageId);

            builder.Property(x => x.Role)
                   .HasConversion<string>()
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.Content)
                   .IsRequired();

            builder.Property(x => x.PromptTokens);

            builder.Property(x => x.CompletionTokens);

            builder.Property(x => x.TotalTokens);

            builder.Property(x => x.CreatedOn)
                   .IsRequired();
        }
    }
}
