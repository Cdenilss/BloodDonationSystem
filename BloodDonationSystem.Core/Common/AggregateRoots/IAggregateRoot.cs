using BloodDonationSystem.Core.Common.DomainEvents;

namespace BloodDonationSystem.Core.Common.AggregateRoots;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}