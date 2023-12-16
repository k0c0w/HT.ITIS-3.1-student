namespace Dotnet.Homeworks.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public object? EntityIdentifier { get; }

    public Type EntityType { get; }

    public EntityNotFoundException(Type entityType) 
    {
        EntityIdentifier = entityType;
    }

    public EntityNotFoundException(object entityIdentifier, Type entityType) : this(entityType)
    {
        EntityIdentifier = entityIdentifier;
    }
}
