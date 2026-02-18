using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace MindCalm.Services.Identity.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        return services;
    }
}