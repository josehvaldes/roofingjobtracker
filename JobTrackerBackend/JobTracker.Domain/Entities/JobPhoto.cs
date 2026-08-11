using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Entities
{
    public sealed class JobPhoto
    {
        public Guid Id { get; private set; }
        public string Url { get; private set; } = string.Empty;

        public string Caption { get; private set; } = string.Empty;
        public DateTime CapturedAt { get; private set; }

        public Guid JobId { get; private set; }

        private JobPhoto() { }

        internal static JobPhoto Create(Guid jobId, string url, DateTime capturedAt, string caption)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("Photo URL is required.", nameof(url));
            }

            return new JobPhoto
            {
                Id = Guid.NewGuid(),
                JobId = jobId,
                Url = url,
                CapturedAt = capturedAt,
                Caption = caption ?? string.Empty
            };
        }
    }
}
