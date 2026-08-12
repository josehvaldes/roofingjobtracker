using JobTracker.Application.Common.Interfaces;
using MediatR;

namespace JobTracker.Application.Common.Behaviors
{
    public class UnitOfWorkBehavior<TRequest, TResponse>(IEnumerable<IUnitOfWork> unitOfWorks)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (request is not IBaseCommand)
                return await next();

            var response = await next();
            foreach (var unitOfWork in unitOfWorks)
            {
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            return response;
        }
    }
}
