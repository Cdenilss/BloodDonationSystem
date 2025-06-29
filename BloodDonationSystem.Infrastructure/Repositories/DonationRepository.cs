using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationSystem.Infrastructure.Repositories;

public class DonationRepository : IRepositoryDonation
{
     
    private readonly BloodDonationDbContext _context;
    private readonly IRepositoryDonor _donorRepository;

   
  
    public DonationRepository(BloodDonationDbContext context, IRepositoryDonor donorRepository)
    {
        _context = context;
        _donorRepository = donorRepository;
    }
    
    public async Task<List<Donation>> GetAll()
    {
        var donations = await _context.Donations
            .Where(d => d.IsDeleted == false)
            .Include(d => d.Donor)
            .AsNoTracking()
            .ToListAsync();
        return donations;
    }

    public async Task<Donation> GetById(Guid id)
    {
        var donation = await _context.Donations
            .FirstOrDefaultAsync(d => d.Id == id);
        return donation;
    }

    public async Task<List<Donation>> GetByDonorId(Guid donorId)
    {
        return await _context.Donations
            .Where(d => d.DonorId == donorId) 
            .AsNoTracking() 
            .ToListAsync();
    }

    public async Task Add(Donation donation)
    {
        await _context.Donations.AddAsync(donation);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var donation = await GetById(id);
            donation.SetAsDeleted();
            await _context.SaveChangesAsync();
        
    }
}