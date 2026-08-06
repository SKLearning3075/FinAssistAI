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
    public class DocumentConfiguration : IEntityTypeConfiguration<DocumentEntity>
    {
        public void Configure(EntityTypeBuilder<DocumentEntity> builder)
        {
           builder.ToTable("Documents");
            builder.HasKey(d => d.DocumentId);
            builder.Property(d => d.UserId)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(d => d.OriginalFileName)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(d => d.StoredFilePath)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(d => d.ContentType)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(d => d.FileSize)
                .IsRequired();
            builder.Property(d => d.Status)
                .IsRequired();
            builder.Property(d => d.UploadedOn)
                .IsRequired();
        }
    }
}
