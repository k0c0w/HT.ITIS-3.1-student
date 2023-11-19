namespace Dotnet.Homeworks.Mediator;

internal class MediatorRequestToHandlerMap
{
    private readonly Dictionary<Type, Type> _map;

    public MediatorRequestToHandlerMap(Dictionary<Type, Type> map)
    {
        _map = map;
    }

    public Type this[Type key] => _map[key];

    public bool ContainsKey(Type key) => _map.ContainsKey(key);
}
