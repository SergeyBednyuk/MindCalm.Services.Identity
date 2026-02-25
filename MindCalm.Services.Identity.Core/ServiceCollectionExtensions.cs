using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MindCalm.Services.Identity.Core.Common.Behaviors;
using MindCalm.Services.Identity.Core.Features.Auth.Login.GuestLogin;

namespace MindCalm.Services.Identity.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // 1. Register MediatR AND the Validation Pipeline Behavior
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(GuestLoginCommand).Assembly));

        // 2. Register all FluentValidation Validators in this project automatically
        services.AddValidatorsFromAssembly(assembly);
        
        return services;
    }
}