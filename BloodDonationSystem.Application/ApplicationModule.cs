using BloodDonationSystem.Application.Common.Mediator; // Para AddCustomMediator e IMediator
using FluentValidation; // Para AddValidatorsFromAssembly
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BloodDonationSystem.Application.Events;
using BloodDonationSystem.Application.Events.BloodStockEvent;
using BloodDonationSystem.Application.Validators.DonationValidator;
using BloodDonationSystem.Core.Common.DomainEvents;
using BloodDonationSystem.Core.Common.Events.BloodStockEvents;

namespace BloodDonationSystem.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddCustomMediatorFromAssembly();
        services.AddValidatorsFromAssembly(typeof(ApplicationModule).Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddScoped<IDonationEligibilityValidator, DonationEligibilityValidator>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IDomainEventHandler<BloodStockBecameLowEvent>, BloodStockBecameLowEventHandler>();


        return services;
    }

    private static IServiceCollection AddCustomMediatorFromAssembly(this IServiceCollection services)
    {
        services.AddCustomMediator(typeof(ApplicationModule).Assembly);
        return services;
    }
}