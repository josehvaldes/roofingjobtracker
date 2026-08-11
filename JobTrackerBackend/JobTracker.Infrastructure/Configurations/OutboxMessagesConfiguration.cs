using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobTracker.Infrastructure.Configurations
{
    internal class OutboxMessagesConfiguration: IEntityTypeConfiguration<OutboxMessages>, IEntityConfiguration
    {
        public void Configure(EntityTypeBuilder<OutboxMessages> builder)
        {
            builder.ToTable("outbox_messages", schema: "jobs");

            builder.HasKey(message => message.Id);

            builder.Property(message => message.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(message => message.Type)
                .HasColumnName("type")
                .IsRequired();

            builder.Property(message => message.Content)
                .HasColumnName("content")
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(message => message.OccurredOn)
                .HasColumnName("occurred_on")
                .IsRequired();

            builder.Property(message => message.ProcessedOn)
                .HasColumnName("processed_on");

            builder.Property(message => message.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.HasIndex(message => message.OccurredOn)
                .HasDatabaseName("idx_jobs_outbox_unprocessed")
                .HasFilter("processed_on IS NULL");
        }
    }
}
