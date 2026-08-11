using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobTracker.Infrastructure.Configurations
{
    public class JobPhotoConfiguration : IEntityTypeConfiguration<JobPhoto>, IEntityConfiguration
    {
        public void Configure(EntityTypeBuilder<JobPhoto> builder)
        {
            builder.ToTable("job_photos", schema: "jobs");

            builder.HasKey(photo => photo.Id);

            builder.Property(photo => photo.Id)
                .HasColumnName("id");

            builder.Property(photo => photo.JobId)
                .HasColumnName("job_id")
                .IsRequired();

            builder.Property(photo => photo.Url)
                .HasColumnName("url")
                .IsRequired();

            builder.Property(photo => photo.CapturedAt)
                .HasColumnName("captured_at")
                .IsRequired();

            builder.Property(photo => photo.Caption)
                .HasColumnName("caption");

            builder.HasIndex(photo => photo.JobId)
                .HasDatabaseName("idx_jobs_job_photos_job_id");
        }
    }
}
