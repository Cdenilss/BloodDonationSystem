using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Core.Repositories;

public interface IRepositoryDonor
{
    Task<List<Donor>> GetAll();
    Task<Donor> GetById(Guid id);
    
    Task<Donor> GetDonorByEmail(string email);
    Task<Donor> GetDetailsById(Guid id);
    Task Add(Donor donor);
    Task Update(Donor donor);
    Task Delete(Guid id);
    
}