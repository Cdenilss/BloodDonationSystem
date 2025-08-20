using System.Reflection;
using BloodDonationSystem.Application.Events;
using BloodDonationSystem.Core.Common.AggregateRoots;
using BloodDonationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationSystem.Infrastructure.Persistence;

public class BloodDonationDbContext : DbContext
{
    // protected BloodDonationDbContext()
    // {
    // }
    private readonly IDomainEventDispatcher _dispatcher;

    public BloodDonationDbContext(DbContextOptions options, IDomainEventDispatcher dispatcher)
        : base(options) => _dispatcher = dispatcher;

    public DbSet<Donor> Donors { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<BloodStock> BloodStocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var result = await base.SaveChangesAsync(ct);
        await PublishDomainEventsAsync(ct);
        return result;
    }


    private async Task PublishDomainEventsAsync(CancellationToken ct)
    {
        var aggregates = ChangeTracker.Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = aggregates.SelectMany(a => a.DomainEvents).ToList();
        aggregates.ForEach(a => a.ClearDomainEvents());
        if (events.Count > 0)
        {
            Console.WriteLine(
                $"[DOMAIN] Dispatching {events.Count} event(s): {string.Join(", ", events.Select(e => e.GetType().Name))}");
        }

        await _dispatcher.DispatchAsync(events, ct);
    }
}