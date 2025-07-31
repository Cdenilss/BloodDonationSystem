using BloodDonationSystem.Application.Services.ViaCep;
using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Infrastructure.Persistence;
using BloodDonationSystem.Infrastructure.Repositories;
using BloodDonationSystem.Infrastructure.Services.ViaCepServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BloodDonationSystem.Infrastructure;

public static class InfraModule
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
     
        services.AddDatabase(configuration);
        services.AddRepositoryies();
        services.AddUnitOfWork();
        services.AddExternalServices(configuration);
        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        
        var connectionString = configuration.GetConnectionString("BloodDonationDb");
        services.AddDbContext<BloodDonationDbContext>(o => o.UseSqlServer(connectionString));
        return services;
        
    }
    private static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        var viaCepBaseUrl = configuration.GetSection("ExternalServices:ViaCep:BaseUrl").Value;
        if (string.IsNullOrEmpty(viaCepBaseUrl))
        {
            throw new InvalidOperationException("URL base para o serviço ViaCep não encontrada.");
        }
        services.AddHttpClient("ViaCep", client => { client.BaseAddress = new Uri(viaCepBaseUrl); });
        services.AddScoped<IViaCepService, ViaCepService>();
        return services;
    }
    public static IServiceCollection AddRepositoryies(this IServiceCollection services)
    {
        services.AddScoped<IDonorRepository,DonorRepository>();
        services.AddScoped<IDonationRepository,DonationRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IBloodStockRepository, BloodStockRepository>(); 
        return services;
    }

    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
        
    }
}