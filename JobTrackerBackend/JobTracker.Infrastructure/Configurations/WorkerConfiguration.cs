using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobTracker.Infrastructure.Configurations
{
    internal class WorkerConfiguration : IEntityTypeConfiguration<Worker>, IEntityConfiguration
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.ToTable("workers", schema: "jobs");

            builder.HasKey(worker => worker.Id);

            builder.Property(worker => worker.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(worker => worker.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(worker => worker.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(worker => worker.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(20);

            builder.Property(worker => worker.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.Property(worker => worker.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.HasIndex(worker => worker.Email)
                .IsUnique();
        }
    }
}
