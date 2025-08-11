using BloodDonationSystem.Core.Common;
using BloodDonationSystem.Core.DomainEvents;
using BloodDonationSystem.Core.Enum;
using BloodDonationSystem.Core.Events.DomainBloodStocks;

namespace BloodDonationSystem.Core.Entities;

public class BloodStock: BaseEntity , IAggregateRoot
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
    

    public void Draw(int ml)
    {
        if (ml <= 0) throw new ArgumentOutOfRangeException(nameof(ml));
        if (ml > QuantityMl) throw new InvalidOperationException("Estoque insuficiente.");

        QuantityMl -= ml;

        if (QuantityMl < MinimumSafeQuantity)
            AddDomainEvent(BloodStockBecameLowEvent.From(this));
    }
}

