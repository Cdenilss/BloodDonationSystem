namespace BloodDonationSystem.Core.Repositories;

public interface IUnitOfWork : IDisposable
{
    
    IDonationRepository Donations { get; }
    IBloodStockRepository BloodStocks { get; }
    
    IDonorRepository Donors { get; }
    
    Task<int> CompleteAsync();
}