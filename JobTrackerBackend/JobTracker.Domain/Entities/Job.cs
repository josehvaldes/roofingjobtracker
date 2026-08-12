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
                
                Status = Status.Draft, // Default status is Draft

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

        private string FindTransitionJobError(Status current, Status newStatus)
        {
            if (current == Status.Draft && (newStatus != Status.Scheduled && newStatus != Status.Cancelled))
            {
                return "Draft jobs can only transition to Scheduled or Cancelled.";
            }
            if (current == Status.Scheduled && (newStatus != Status.InProgress && newStatus != Status.Cancelled))
            {
                return "Scheduled jobs can only transition to InProgress or Cancelled.";
            }
            if (current == Status.InProgress && (newStatus != Status.Completed && newStatus != Status.Cancelled))
            {
                return "InProgress jobs can only transition to Completed or Cancelled.";
            }
            if ((current == Status.Completed || current == Status.Cancelled) && newStatus != current)
            {
                return "Completed or Cancelled jobs cannot transition to any other status.";
            }
            return String.Empty;
        }

        public void UpdateStatus(Status newStatus)
        {
            var transitionError = FindTransitionJobError(Status, newStatus);
            if (transitionError != string.Empty)
            {
                throw new InvalidJobTransitionException(this.Id, transitionError);
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
