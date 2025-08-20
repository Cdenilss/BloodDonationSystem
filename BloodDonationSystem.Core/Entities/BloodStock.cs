using BloodDonationSystem.Core.Common.Events.BloodStockEvents;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Core.Entities;

public class BloodStock : BaseEntity
{
    public BloodStock(BloodTypeEnum bloodType, RhFactorEnum rhFactor, int quantityMl)
    {
        BloodType = bloodType;
        RhFactor = rhFactor;
        QuantityMl = quantityMl;
        MinimumSafeQuantity = 1500;
    }
    
    public BloodTypeEnum BloodType { get; set; }
    public RhFactorEnum RhFactor { get; set; }
    public int QuantityMl { get; set; }
    public int MinimumSafeQuantity { get; set; }


    public void Draw(int ml)
    {
        QuantityMl -= ml;

        if (ml <= 0)
            throw new ArgumentException("A quantidade retirada deve ser maior que zero.", nameof(ml));

        if (ml > QuantityMl)
            throw new InvalidOperationException("Não é possível retirar mais sangue do que há em estoque.");

        if (QuantityMl <= MinimumSafeQuantity)
        {
            AddDomainEvent(new BloodStockBecameLowEvent(
                Id, BloodType, RhFactor, QuantityMl, MinimumSafeQuantity));
        }
    }
}