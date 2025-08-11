using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Core.Events.DomainBloodStocks;

namespace BloodDonationSystem.Application.Events.BloodStock;

public class BloodStockBecameLowEventHandler: IDomainEventHandler<BloodStockBecameLowEvent>
{
    public Task Handle(BloodStockBecameLowEvent domainEvent, CancellationToken cancellationToken)
    {
        // Logic to handle the event when blood stock becomes low
        // For example, sending a notification or logging the event

        Console.WriteLine($"Blood stock with BloodType {domainEvent.BloodType} Rh{domainEvent.RhFactor} has become low.");
        
        // You can also add more complex logic here, such as notifying relevant parties
        // or triggering other processes in the system.

        return Task.CompletedTask;
    }
}