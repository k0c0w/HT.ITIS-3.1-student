using MediatR;

namespace Dotnet.Homeworks.Infrastructure.Cqrs.Queries;

public interface IQuery<TResponse> : IRequest<TResponse> 
{
}