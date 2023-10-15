using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using MediatR;

namespace Dotnet.Homeworks.Infrastructure.Cqrs.Queries;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
}