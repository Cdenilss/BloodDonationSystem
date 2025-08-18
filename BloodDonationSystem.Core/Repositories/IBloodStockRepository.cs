using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Core.Repositories;

public interface IBloodStockRepository
{
    Task<List<BloodStock>> GetAllAsync();
        
   
    Task<BloodStock?> GetByTypeAsync(BloodTypeEnum bloodType, RhFactorEnum rhFactor);
        
    
    Task AddAsync(BloodStock bloodStock);


    void UpdateAsync(BloodStock bloodStock);
    
}