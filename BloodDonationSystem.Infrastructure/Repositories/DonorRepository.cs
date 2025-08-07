using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationSystem.Infrastructure.Repositories;

public class DonorRepository : IDonorRepository

{
    private readonly BloodDonationDbContext _context;
    public DonorRepository(BloodDonationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Donor>> GetAll()
    {
        var donors = await _context.Donors
            .Where(d => d.IsDeleted == false)
            
            .AsNoTracking()
            .ToListAsync();
        return donors;
    }

    public async Task<Donor> GetById(Guid id)
    {
        var donor= await _context.Donors.FirstOrDefaultAsync(d=> d.Id==id);
        return donor;
    }

    public async Task<Donor> GetDonorByEmail(string email)
    {
        var donor = await _context.Donors
            .Include(d => d.Address)
            .Include(d => d.Donations.Where(dd=> dd.IsDeleted == false))
            .FirstOrDefaultAsync(d => d.Email== email);
        return donor;
    }

    public async Task<Donor?> GetDetailsById(Guid id)
    {
        var donor = await _context.Donors
            .Include(d => d.Address)
            .Include(d => d.Donations.Where(dd=> dd.IsDeleted == false))
            .SingleOrDefaultAsync(d => d.Id == id);
        return donor;
    }

    public async Task Add(Donor donor)
    {
        await _context.Donors.AddAsync(donor);
    }

    public Task Update(Donor donor)
    {
        _context.Donors.Update(donor);
        return Task.CompletedTask;
    }

    public async Task Delete(Guid id)
    {
        var donor = await GetById(id);
        donor.SetAsDeleted();
    }
}