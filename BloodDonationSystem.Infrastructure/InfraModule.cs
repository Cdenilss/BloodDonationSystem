using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Infrastructure.Persistence;
using BloodDonationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BloodDonationSystem.Infrastructure;

public static class InfraModule
{
    public static IServiceCollection AddInfrastrucutre(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositoryies();
        services.AddDatabase(configuration);
        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        
        var connectionString = configuration.GetConnectionString("BloodDonationDb");
        services.AddDbContext<BloodDonationDbContext>(o => o.UseSqlServer(connectionString));
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