using BloodDonationSystem.Core.DomainEvents;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Core.Events.DomainBloodStocks;

public sealed record BloodStockBecameLowEvent(
    Guid BloodStockId,
    BloodTypeEnum BloodType,
    RhFactorEnum RhFactor,
    int QuantityMl,
    int MinimumSafeQuantity
) : DomainEvent
{
    public static BloodStockBecameLowEvent From(BloodStock s) =>
        new(s.Id, s.BloodType, s.RhFactor, s.QuantityMl, s.MinimumSafeQuantity);
}