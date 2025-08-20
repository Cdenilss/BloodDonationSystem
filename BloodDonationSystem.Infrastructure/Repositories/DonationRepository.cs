using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationSystem.Infrastructure.Repositories;

public class DonationRepository : IDonationRepository
{
    private readonly BloodDonationDbContext _context;
    private readonly IDonorRepository _donorRepository;


    public DonationRepository(BloodDonationDbContext context, IDonorRepository donorRepository)
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
    }

    public async Task Delete(Guid id)
    {
        var donation = await GetById(id);
        donation.SetAsDeleted();
    }

    public async Task<Donation?> GetLastByDonorIdAsync(Guid donorId)
    {
        return await _context.Donations
            .Where(d => d.DonorId == donorId && d.IsDeleted == false)
            .OrderByDescending(d => d.DateDonation)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Donation>> GetAllLast30DaysDonation()
    {
        var donations = await _context.Donations
            .Where(d => d.IsDeleted == false)
            .Where(d => d.DateDonation >= DateTime.Now.AddDays(-30))
            .OrderByDescending(d => d.DateDonation)
            .Include(d => d.Donor)
            .AsNoTracking()
            .ToListAsync();

        return donations;
    }
}