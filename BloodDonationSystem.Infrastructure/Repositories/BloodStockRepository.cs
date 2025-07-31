 
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;
using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Infrastructure.Persistence;
using BloodDonationSystem.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationSystem.Infrastructure.Repositories;

public class BloodStockRepository : IBloodStockRepository
{

    private readonly BloodDonationDbContext _context;

    public BloodStockRepository(BloodDonationDbContext context)
    {
        _context = context;
    }

    
    public async Task Add(BloodStock bloodStock)
    {
        await _context.BloodStocks.AddAsync(bloodStock);
    
    }


    public async Task<List<BloodStock>> GetAllAsync()
    {
        var bloodStocks = await _context.BloodStocks
            .Where(bs => bs.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();
        return bloodStocks;
    }

    public async Task<BloodStock?> GetByTypeAsync(BloodTypeEnum bloodType, RhFactorEnum rhFactor)
    {
       var bloodStock= await _context.BloodStocks.FirstOrDefaultAsync(bs=> bs.BloodType == bloodType && bs.RhFactor == rhFactor);
       
       return bloodStock;
    }

    public async Task AddAsync(BloodStock bloodStock)
    {
        await _context.AddAsync(bloodStock);
        
    }
    
    public void UpdateAsync(BloodStock bloodStock)
    {
        
        _context.BloodStocks.Update(bloodStock);
    }
}