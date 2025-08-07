using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Core.Events.DomainBloodStocks;

public sealed record BloodStockBecameLowEvent : DomainEvent
{
    
    public BloodStockBecameLowEvent(BloodStock bloodStock)
    {
        BloodStock = bloodStock;
    }

   public Guid BloodStockId => BloodStock.Id;

    public BloodStock BloodStock { get; }
    
}