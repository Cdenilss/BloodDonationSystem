 
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Infrastructure.Persistence;
using BloodDonationSystem.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationSystem.Infrastructure.Repositories;

public class BloodStockRepository : IRepositoryBloodStock
{
        
  private readonly BloodDonationDbContext _context;
    public BloodStockRepository(BloodDonationDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<List<BloodStock>> GetAll()
    {
        var bloodStocks = await _context.BloodStocks
            .Where(bs=>bs.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();
        return bloodStocks;
    }

    public async Task<BloodStock> GetById(Guid id)
    {
        var bloodStock = await _context.BloodStocks.FirstOrDefaultAsync(bs => bs.Id == id);
        return bloodStock;
    }

    public async Task Add(BloodStock bloodStock)
    {
       await _context.BloodStocks.AddAsync(bloodStock);
       await _context.SaveChangesAsync();
    }

    public async Task Update(BloodStock bloodStock)
    {
        _context.Update(bloodStock);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var bloodStockDelete = await GetById(id);
        bloodStockDelete.SetAsDeleted();
        await _context.SaveChangesAsync();
        
    }


}