using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobTracker.Infrastructure.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>, IEntityConfiguration
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("jobs", schema: "jobs");

            builder.HasKey(job => job.Id);

            builder.Property(job => job.Id)
                .HasColumnName("id");

            builder.Property(job => job.Title)
                .HasColumnName("title")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(job => job.Description)
                .HasColumnName("description");

            builder.Property(job => job.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();

            builder.Property(job => job.ScheduledDate)
                .HasColumnName("scheduled_date");

            builder.Property(job => job.AssigneeId)
                .HasColumnName("assignee_id");

            builder.Property(job => job.CustomerId)
                .HasColumnName("customer_id")
                .IsRequired();

            builder.Property(job => job.OrganizationId)
                .HasColumnName("organization_id")
                .IsRequired();

            builder.Property(job => job.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(job => job.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            builder.Ignore(job => job.DomainEvents);

            ConfigureAddress(builder);

            builder.Navigation(job => job.Address)
                .IsRequired();

            builder.HasOne<Worker>()
                .WithMany()
                .HasForeignKey(job => job.AssigneeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(job => job.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<Organization>()
                .WithMany()
                .HasForeignKey(job => job.OrganizationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(job => job.JobPhotos)
                .WithOne()
                .HasForeignKey(photo => photo.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(job => job.JobPhotos)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            ConfigureIndexes(builder);
        }

        private static void ConfigureAddress(EntityTypeBuilder<Job> builder)
        {
            builder.OwnsOne(job => job.Address, addressBuilder =>
            {
                addressBuilder.Property(address => address.Street)
                    .HasColumnName("street")
                    .HasMaxLength(255)
                    .IsRequired();

                addressBuilder.Property(address => address.City)
                    .HasColumnName("city")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(address => address.State)
                    .HasColumnName("state")
                    .HasMaxLength(100)
                    .IsRequired();

                addressBuilder.Property(address => address.ZipCode)
                    .HasColumnName("zip_code")
                    .HasMaxLength(20)
                    .IsRequired();

                addressBuilder.Property(address => address.Latitude)
                    .HasColumnName("latitude")
                    .HasPrecision(9, 6)
                    .IsRequired();

                addressBuilder.Property(address => address.Longitude)
                    .HasColumnName("longitude")
                    .HasPrecision(9, 6)
                    .IsRequired();
            });
        }

        private static void ConfigureIndexes(EntityTypeBuilder<Job> builder)
        {
            builder.HasIndex(job => job.OrganizationId)
                .HasDatabaseName("idx_jobs_jobs_organization_id");

            builder.HasIndex(job => new { job.OrganizationId, job.Status })
                .HasDatabaseName("idx_jobs_jobs_org_status");

            builder.HasIndex(job => new { job.OrganizationId, job.ScheduledDate, job.Id })
                .HasDatabaseName("idx_jobs_jobs_org_scheduled_date_id");
        }
    }
}
