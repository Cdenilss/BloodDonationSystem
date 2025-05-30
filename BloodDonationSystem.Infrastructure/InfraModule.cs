using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BloodDonationSystem.Infrastructure;

public static class InfraModule
{
    public static IServiceCollection AddInfrastrucutre(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositoryies();
        return services;
    }
    
    public static IServiceCollection AddRepositoryies(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryDonor,DonorRepository>();
        services.AddScoped<IRepositoryDonation,DonationRepository>();
        services.AddScoped<IRepositoryAddress, AddressRepository>();
        return services;
    }
}