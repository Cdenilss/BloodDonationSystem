using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Core.Repositories;

public interface IBloodStockRepository
{
    Task<List<BloodStock>> GetAllAsync();
        
    // Busca um estoque pela sua chave composta (o tipo e fator Rh)
    Task<BloodStock?> GetByTypeAsync(BloodTypeEnum bloodType, RhFactorEnum rhFactor);
        
    // Adiciona um novo registro de estoque ao contexto
    Task AddAsync(BloodStock bloodStock);

    // Marca um registro de estoque como modificado no contexto
    void UpdateAsync(BloodStock bloodStock);
}