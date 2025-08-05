using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Application.Models.DTO;
public class BloodStockItemViewModel
{
    public BloodStockItemViewModel(Guid id, BloodTypeEnum bloodType, RhFactorEnum rhFactor, int quantityMl, int minimumSafeQuantity)
    {
        Id = id;
        BloodType = bloodType;
        RhFactor = rhFactor;
        QuantityMl = quantityMl;
        MinimumSafeQuantity = minimumSafeQuantity;
    }

    public Guid Id { get; set; }
    public BloodTypeEnum BloodType { get; private set; }
    public RhFactorEnum RhFactor { get; private set; }
    public int QuantityMl { get; private set; }
    public int MinimumSafeQuantity { get; private set; }

    public static BloodStockItemViewModel FromEntity(BloodStock stock)
        => new(stock.Id, stock.BloodType, stock.RhFactor, stock.QuantityMl, stock.MinimumSafeQuantity);
}