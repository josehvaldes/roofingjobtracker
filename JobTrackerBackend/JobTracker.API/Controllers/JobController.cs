using Asp.Versioning;
using FluentValidation;
using JobTracker.API.Extensions;
using JobTracker.Application.Features.Jobs.Commands.CompleteJob;
using JobTracker.Application.Features.Jobs.Commands.CreateJob;
using JobTracker.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class JobController(IMediator mediator,
        ILogger<JobController> logger,
        IValidator<CreateJobRequest> createJobRequestValidator
        ) : ControllerBase
    {
        [HttpGet("/health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { Status = "Healthy" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await createJobRequestValidator.ValidateAsync(request, cancellationToken);
            validationResult.ThrowIfInvalid();

            var command = new CreateJobCommand(
                request.Title,
                request.Description,
                request.Street,
                request.City,
                request.State,
                request.ZipCode,
                request.Latitude,
                request.Longitude,
                Guid.Parse(request.AssigneeId),
                Guid.Parse(request.CustomerId),
                Guid.Parse(request.OrganizationId)
                );
            var jobId = await mediator.Send(command, cancellationToken);
            logger.LogInformation("Job created with ID: {JobId}", jobId);
            return CreatedAtAction(nameof(GetJob), new { id = jobId }, jobId);
        }

        [HttpPatch("complete")]
        public async Task<IActionResult> CompleteJob([FromBody] CompleteJobRequest request, CancellationToken cancellationToken)
        {
            var command = new CompleteJobCommand(request.JobId);
            await mediator.Send(command, cancellationToken);

            return Accepted(new { JobId = request.JobId });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetJob(Guid id, CancellationToken cancellationToken)
        {
            //out of scope for now, just return the id
            return Ok(new { JobId = id });
        }
    }
}
