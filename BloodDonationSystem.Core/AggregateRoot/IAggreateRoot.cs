using BloodDonationSystem.Core.Events;

namespace BloodDonationSystem.Core.Common;

public interface IAggregateRoot
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    void AddDomainEvent(DomainEvent domainEvent);
    void ClearDomainEvents();
}