
using BloodDonationSystem.Core.Common.Events.BloodStockEvents;
using Microsoft.Extensions.Logging;


namespace BloodDonationSystem.Application.Events.BloodStockEvent;


public sealed class BloodStockBecameLowEventHandler
    : IDomainEventHandler<BloodStockBecameLowEvent>
{
    private readonly ILogger<BloodStockBecameLowEventHandler> _logger;
    public BloodStockBecameLowEventHandler(ILogger<BloodStockBecameLowEventHandler> logger)
        => _logger = logger;

    public Task Handle(BloodStockBecameLowEvent e, CancellationToken ct)
    {
        _logger.LogWarning("Estoque baixo {Type}{Rh}: {Qty}ml (mín {Min})",
            e.BloodType, e.RhFactor, e.QuantityMl, e.MinimumSafeQuantity);
        return Task.CompletedTask;
    }
}
