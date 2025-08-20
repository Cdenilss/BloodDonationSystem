namespace BloodDonationSystem.Core.Common.DomainEvents;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}