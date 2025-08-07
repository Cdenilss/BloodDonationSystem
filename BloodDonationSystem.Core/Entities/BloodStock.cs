using BloodDonationSystem.Core.DomainEvents;
using BloodDonationSystem.Core.Enum;
using BloodDonationSystem.Core.Events.DomainBloodStocks;

namespace BloodDonationSystem.Core.Entities;

public class BloodStock: BaseEntity
{
    public BloodStock(BloodTypeEnum bloodType, RhFactorEnum rhFactor, int quantityMl)
    {
        BloodType = bloodType;
        RhFactor = rhFactor;
        QuantityMl = quantityMl;
    }
   
    public BloodTypeEnum BloodType { get; private set; }
    public RhFactorEnum RhFactor { get; private set; }
    public int QuantityMl { get; set;}
    public int MinimumSafeQuantity { get; set; }
    

    public BloodStock()
    {

        AddDomainEvent(new BloodStockBecameLowEvent(this));
    }
}

