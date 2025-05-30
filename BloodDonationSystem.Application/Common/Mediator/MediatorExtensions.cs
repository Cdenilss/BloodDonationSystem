namespace BloodDonationSystem.Application.Common.Mediator; 

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

public static class MediatorExtensions
{
   
    public static IServiceCollection AddCustomMediator(this IServiceCollection services, Assembly assemblyToScan)
    {
       
        services.AddScoped<IMediator, Mediator>();
        var handlerTypes = assemblyToScan.GetTypes()
            .Where(t => t.GetInterfaces().Any(IsHandlerInterface) && !t.IsAbstract && !t.IsInterface);

        foreach (var type in handlerTypes)
        {
            var interfaces = type.GetInterfaces().Where(IsHandlerInterface);
            foreach (var interfaceType in interfaces)
            {
                services.AddScoped(interfaceType, type); 
            }
        }

        return services;
    }

    private static bool IsHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
            return false;

        var genericTypeDefinition = type.GetGenericTypeDefinition();
        return genericTypeDefinition == typeof(IRequestHandler<>) || genericTypeDefinition == typeof(IRequestHandler<,>);
    }
}