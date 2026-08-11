using JobTracker.Domain.Enums;
using JobTracker.Domain.Events;
using JobTracker.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobTracker.Domain.Entities
{
    public class Job : BaseEntity
    {
        private readonly List<JobPhoto> _jobPhotos = new();

        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        public Address Address { get; private set; } = default!;

        public Status Status { get; private set; } = Status.Draft;

        public DateTime? ScheduledDate { get; private set; }

        public Guid AssigneeId { get; private set; }

        public Guid CustomerId { get; private set; }

        public Guid OrganizationId { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public IReadOnlyCollection<JobPhoto> JobPhotos => _jobPhotos.AsReadOnly();

        private Job() { }

        public static Job CreateJob(string title,
            string description,
            Address address,
            Guid assigneeId,
            Guid customerId,
            Guid organizationId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidEntityException("Job title is required.");
            }

            if (address is null)
            {
                throw new InvalidEntityException("Address is required.");
            }

            var job = new Job()
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                Address = address,
                AssigneeId = assigneeId,
                CustomerId = customerId,
                OrganizationId = organizationId,

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            job._domainEvents.Add(new JobCreatedDomainEvent(
                job.Id, 
                job.Title, 
                job.Description, 
                job.AssigneeId, 
                job.CustomerId, 
                job.OrganizationId));

            return job;
        }

        public void UpdateJob(string title,
            string description,
            Guid assigneeId,
            Guid customerId,
            Guid organizationId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidEntityException("Job title is required.");
            }

            Title = title;
            Description = description;
            AssigneeId = assigneeId;
            CustomerId = customerId;
            OrganizationId = organizationId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateAddress(Address address)
        {
            if (address is null)
            {
                throw new InvalidEntityException("Address is required.");
            }

            Address = address;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateScheduledDate(DateTime scheduledDate)
        {
            if (scheduledDate < DateTime.UtcNow)
            {
                throw new InvalidEntityException("Scheduled date cannot be in the past.");
            }
            ScheduledDate = scheduledDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(Status newStatus)
        {
            if (this.Status == Status.Completed || this.Status == Status.Cancelled)
            {
                throw new InvalidEntityException("Cannot update a job that is already completed or cancelled.");
            }

            if (newStatus == Status.InProgress && this.Status != Status.Scheduled)
            {
                throw new InvalidEntityException("Only scheduled jobs can move to InProgress.");
            }

            if (newStatus == Status.InProgress && this.ScheduledDate == null)
            {
                throw new InvalidEntityException("Cannot set a job to InProgress without a scheduled date.");
            }

            if (newStatus == Status.Completed)
            { 
                _domainEvents.Add(new JobCompletedDomainEvent(
                    this.Id
                    ));
            }
            if (newStatus == Status.Cancelled) 
            { 
                _domainEvents.Add(new JobCancelledDomainEvent(
                    this.Id
                    ));
            }
            
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddPhoto(string url, DateTime capturedAt, string caption = "")
        {
            var photo = JobPhoto.Create(this.Id, url, capturedAt, caption);
            _jobPhotos.Add(photo);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemovePhoto(Guid photoId)
        {
            var photo = _jobPhotos.FirstOrDefault(p => p.Id == photoId);
            if (photo == null)
            {
                throw new InvalidEntityException($"Job photo {photoId} was not found.");
            }

            _jobPhotos.Remove(photo);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
