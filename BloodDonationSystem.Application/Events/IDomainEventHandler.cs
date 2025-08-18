using BloodDonationSystem.Core.Common.DomainEvents;

namespace BloodDonationSystem.Application.Events;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent domainEvent, CancellationToken cancellationToken);
}