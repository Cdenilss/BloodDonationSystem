using System.Reflection;
using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Core.Common;
using BloodDonationSystem.Core.DomainEvents;
using BloodDonationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationSystem.Infrastructure.Persistence;

public class BloodDonationDbContext : DbContext
{
   private readonly IMediator _mediator;

    protected BloodDonationDbContext()
    {
    }

    public BloodDonationDbContext(DbContextOptions options, IMediator mediator) : base(options)
    {
       _mediator = mediator;
    }
    
    
    public DbSet<Donor> Donors { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<BloodStock>BloodStocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        
        var result= await base.SaveChangesAsync(cancellationToken); 
        
        await PublishDomainEvents(cancellationToken);
        
        return result;
    }
    
    private async Task PublishDomainEvents(CancellationToken ct)
    {
        var domainEntities = ChangeTracker.Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = domainEntities.SelectMany(e => e.DomainEvents).ToList();
        domainEntities.ForEach(e => e.ClearDomainEvents());

        foreach (var ev in events)
            await _mediator.PublishDomainEvent(ev, ct);
    }
   
    }
    
   
    
