using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Core.Repositories;

public interface IRepositoryBloodStock
{
    Task<List<BloodStock>> GetAll();
    Task<BloodStock> GetById(Guid id);
    Task Add(BloodStock bloodStock);
    Task Update(BloodStock bloodStock);
    Task Delete(Guid id);
    
}