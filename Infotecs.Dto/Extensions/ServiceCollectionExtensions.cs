using Microsoft.Extensions.DependencyInjection;

namespace Infotecs.Dto.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
        => services.AddAutoMapper(typeof(IAssemblyMarker));
}