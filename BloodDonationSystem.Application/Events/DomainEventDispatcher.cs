using BloodDonationSystem.Core.Common.DomainEvents;
using Microsoft.Extensions.DependencyInjection;

namespace BloodDonationSystem.Application.Events;

public sealed class DomainEventDispatcher : IDomainEventDispatcher
{

    private readonly IServiceProvider _sp;
    public DomainEventDispatcher(IServiceProvider sp) => _sp = sp;

    public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var e in events)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(e.GetType());
            var handlers = _sp.GetServices(handlerType);
            
            // Console.WriteLine($"[DOMAIN] Found {handlers.Count} handler(s) for {e.GetType().Name}");
            
            foreach (var h in handlers)
            {
                var method = handlerType.GetMethod("Handle")!;
                var task = (Task)method.Invoke(h, new object[] { e, cancellationToken })!;
                await task.ConfigureAwait(false);
            }
        }
    }
}
