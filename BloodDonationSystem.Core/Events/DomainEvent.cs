

using BloodDonationSystem.Core.DomainEvents;

namespace BloodDonationSystem.Core.Events;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}