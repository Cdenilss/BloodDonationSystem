using BloodDonationSystem.Core.Common.DomainEvents;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Core.Common.Events.BloodStockEvents;

public sealed record BloodStockBecameLowEvent(
    Guid BloodStockId,
    BloodTypeEnum BloodType,
    RhFactorEnum RhFactor,
    int QuantityMl,
    int MinimumSafeQuantity
) : IDomainEvent;
