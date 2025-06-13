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

        var types = assemblyToScan.GetTypes().Where(t => !t.IsAbstract && !t.IsInterface);

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            foreach (var iface in interfaces)
            {
                if (iface.IsGenericType)
                {
                    var genericDef = iface.GetGenericTypeDefinition();
                    if (genericDef == typeof(IRequestHandler<>) ||
                        genericDef == typeof(IRequestHandler<,>) ||
                        genericDef == typeof(INotificationHandler<>) ||
                        genericDef == typeof(IPipelineBehavior<,>))
                    {
                        services.AddScoped(iface, type);
                    }
                }
            }
        }

        return services;
    }
}