using BloodDonationSystem.Application.Common.Mediator; // Para AddCustomMediator e IMediator
using FluentValidation; // Para AddValidatorsFromAssembly
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BloodDonationSystem.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
   
        services.AddCustomMediatorFromAssembly();
        return services;
    }

    private static IServiceCollection AddCustomMediatorFromAssembly(this IServiceCollection services)
    {
        services.AddCustomMediator(typeof(ApplicationModule).Assembly);
        return services;
    }

    
}