namespace Dotnet.Homeworks.DataAccess.Repositories.Configuration;

public record OrderRepositoryConfiguration
{
    public string CollectionName { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

}
