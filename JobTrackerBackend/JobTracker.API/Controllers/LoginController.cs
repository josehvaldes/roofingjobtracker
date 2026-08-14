using Asp.Versioning;
using FluentValidation;
using JobTracker.API.Extensions;
using JobTracker.Application.Features.Auth.Commands;
using JobTracker.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace JobTracker.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/auth")]
    [ApiVersion("1.0")]
    public class LoginController(IMediator mediator,
        IValidator<LoginRequest> loginValidator,
        ILogger<LoginController> logger) : ControllerBase
    {

        /// <summary>
        /// OAuth-style token endpoint: validates credentials, returns an access token in the
        /// response body and a refresh token in an HttpOnly Secure cookie (not accessible to JS).
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting("AuthEndpoints")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var validationResult = await loginValidator.ValidateAsync(request);
            validationResult.ThrowIfInvalid();

            logger.LogInformation("Login attempt for user: {Username}", request.Username);
            var response = await mediator.Send(new LoginCommand(request.Username, request.Password));
            return Ok(response);
        }
    }
}
