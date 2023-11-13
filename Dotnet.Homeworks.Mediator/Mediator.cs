namespace Dotnet.Homeworks.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, Type> _requestToHandlerMap;

    public Mediator(IServiceProvider serviceProvider, Dictionary<Type, Type> requestTypeToHandlerTypeMap)
    {
        _serviceProvider = serviceProvider;
        _requestToHandlerMap = requestTypeToHandlerTypeMap;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ThrowArgumentNullExceptionIfNull(request, nameof(request));

        var requestType = request.GetType();
        if (_requestToHandlerMap.ContainsKey(requestType))
            throw new InvalidOperationException("Request was not found in registered requests.");
        var handlerType = _requestToHandlerMap[requestType];

        var handler = _serviceProvider.GetService(handlerType);

        var handleRunnerType = typeof(RequestParamsContainer<,>).MakeGenericType(requestType, typeof(TResponse));
        var handleRunner = Activator.CreateInstance(handleRunnerType);

        return handleRunnerType.GetMethod("Handle").Invoke(handleRunner, new object[] { request, handler, cancellationToken }) as Task<TResponse>;
    }
    
    private class RequestParamsContainer<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> Handle(TRequest request, object handler, CancellationToken cancellationToken)
        {
            return ((IRequestHandler<TRequest, TResponse>)handler).Handle(request, cancellationToken);
        }
    }


    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        ThrowArgumentNullExceptionIfNull(request, nameof(request));

        var requestType = request.GetType();
        if (_requestToHandlerMap.ContainsKey(requestType))
            throw new InvalidOperationException("Request was not found in registered requests.");
        var handlerType = _requestToHandlerMap[requestType];

        var handler = _serviceProvider.GetService(handlerType);

        return ((IRequestHandler<TRequest>)handler).Handle(request, cancellationToken);
    }

    public Task<dynamic> Send(dynamic request, CancellationToken cancellationToken = default)
    {
        ThrowArgumentNullExceptionIfNull(request, nameof(request));
        return Send(request, cancellationToken);
    }

    private void ThrowArgumentNullExceptionIfNull<T>(T argument, string argumentName)
    {
        if (argument == null)
            throw new ArgumentNullException(argumentName);
    }
}
