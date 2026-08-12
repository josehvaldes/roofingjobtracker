using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Common.Interfaces
{
    public interface IBaseCommand { }

    public interface ICommand : IRequest<Unit>, IBaseCommand { }

    public interface ICommand<TResponse> : IRequest<TResponse>, IBaseCommand { }
}
