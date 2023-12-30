using Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions;
using Dotnet.Homeworks.Features.Mapster.MapServices.Configuration;

namespace Dotnet.Homeworks.Tests.Mapster;

public partial class MapsterTests
{
    public static IEnumerable<object[]> EnumerateIMappers()
    {
        yield return new object[] { typeof(IOrderMapper) };
        yield return new object[] { typeof(IProductMapper) };
        yield return new object[] { typeof(IUserMapper) };
        yield return new object[] { typeof(IUserManagementMapper) };
    }

    public static IEnumerable<object[]> EnumerateRegisterMappings()
    {
        yield return new object[] { typeof(RegisterOrderMappings) };
        yield return new object[] { typeof(RegisterProductMappings) };
        yield return new object[] { typeof(RegisterUserMappings) };
        yield return new object[] { typeof(RegisterUserManagementMappings) };
    }
}