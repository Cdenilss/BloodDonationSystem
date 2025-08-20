using System.ComponentModel.DataAnnotations.Schema;
using BloodDonationSystem.Core.Common.AggregateRoots;
using BloodDonationSystem.Core.Common.DomainEvents;

namespace BloodDonationSystem.Core.Entities;

public abstract class BaseEntity : IAggregateRoot
{
    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        IsDeleted = false;
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsDeleted { get; set; }

    public void SetAsDeleted()
    {
        IsDeleted = true;
    }

    [NotMapped] private readonly List<IDomainEvent> _domainEvents = new();
    [NotMapped] public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
    public void ClearDomainEvents() => _domainEvents.Clear();
}