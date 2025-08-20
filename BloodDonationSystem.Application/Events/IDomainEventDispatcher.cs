using BloodDonationSystem.Core.Common.DomainEvents;

namespace BloodDonationSystem.Application.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
}