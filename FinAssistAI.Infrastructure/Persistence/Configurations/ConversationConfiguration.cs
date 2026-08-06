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
    public class ConversationConfiguration : IEntityTypeConfiguration<ConversationEntity>
    {
        public void Configure(EntityTypeBuilder<ConversationEntity> builder)
        {
            builder.ToTable("Conversations");

            builder.HasKey(x => x.ConversationId);

            builder.Property(x => x.UserId)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Title)
                   .HasMaxLength(200);

            builder.Property(x => x.CreatedOn)
                   .IsRequired();

            builder.Property(x => x.UpdatedOn)
                   .IsRequired();

            builder.HasMany(x => x.Messages)
                   .WithOne(x => x.Conversation)
                   .HasForeignKey(x => x.ConversationId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
