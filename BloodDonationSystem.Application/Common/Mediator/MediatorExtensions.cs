using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace BloodDonationSystem.Application.Common.Mediator;

public static class MediatorExtensions
{
    public static IServiceCollection AddCustomMediator(this IServiceCollection services, Assembly assemblyToScan)
    {
        services.AddScoped<IMediator, Mediator>();

        var types = assemblyToScan
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && !t.IsGenericType);

        foreach (var implementationType in types)
        {
            var implementedInterfaces = implementationType.GetInterfaces();

            foreach (var serviceType in implementedInterfaces)
            {
                if (!serviceType.IsGenericType) continue;

                var genericDef = serviceType.GetGenericTypeDefinition();

                if (genericDef == typeof(IRequestHandler<>) ||
                    genericDef == typeof(IRequestHandler<,>) ||
                    genericDef == typeof(INotificationHandler<>))
                {
                    services.AddScoped(serviceType, implementationType);
                }
            }
        }

        return services;
    }
}