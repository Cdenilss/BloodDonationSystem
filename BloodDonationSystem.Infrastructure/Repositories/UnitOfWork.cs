using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Infrastructure.Persistence;

namespace BloodDonationSystem.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    
    private readonly BloodDonationDbContext _context;
    
    public UnitOfWork(IDonationRepository donations, IBloodStockRepository bloodStocks, BloodDonationDbContext context, IDonorRepository donors)
    {
        Donations = donations;
        BloodStocks = bloodStocks;
        _context = context;
        Donors = donors;
    }
    public IDonationRepository Donations { get; }
    public IBloodStockRepository BloodStocks { get; }
    public IDonorRepository Donors { get; }

    public async Task<int> CompleteAsync()
    {
      return await _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context?.Dispose();
        }
    }
}