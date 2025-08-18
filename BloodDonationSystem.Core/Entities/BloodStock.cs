using BloodDonationSystem.Core.Common.Events.BloodStockEvents;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Core.Entities;

public class BloodStock: BaseEntity
{
    public BloodStock(BloodTypeEnum bloodType, RhFactorEnum rhFactor, int quantityMl,int minimumSafeQuantity)
    {
        BloodType = bloodType;
        RhFactor = rhFactor;
        QuantityMl = quantityMl;
        MinimumSafeQuantity = 1500;
    }
    
    public BloodTypeEnum BloodType { get; private set; }
    public RhFactorEnum RhFactor { get; private set; }
    public int QuantityMl { get; set;}
    public int MinimumSafeQuantity { get; set; } 
    
    
    public void Draw(int ml)
    {
        QuantityMl -= ml;
        if (QuantityMl <= MinimumSafeQuantity)
        {
            AddDomainEvent(new BloodStockBecameLowEvent(
                Id, BloodType, RhFactor, QuantityMl, MinimumSafeQuantity));
        }
    }
}

