using JobTracker.Application.Common.Interfaces;
using JobTracker.Contracts.Responses;

namespace JobTracker.Application.Features.Auth.Commands
{
    public record LoginCommand(string Username, string Password) : ICommand<LoginResponse>;
}
