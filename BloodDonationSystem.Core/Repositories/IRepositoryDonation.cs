using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Core.Repositories;

public interface IRepositoryDonation
{
    Task<List<Donation>> GetAll();
    Task<Donation> GetById(Guid id);
    Task <List<Donation>> GetByDonorId(Guid donorId);
    Task Add(Donation donation);
    Task Delete(Guid id);
    Task<Donation?> GetLastByDonorIdAsync(Guid donorId);
}