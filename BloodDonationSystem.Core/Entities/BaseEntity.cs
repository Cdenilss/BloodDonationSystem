using System.ComponentModel.DataAnnotations.Schema;
using BloodDonationSystem.Core.Common;
using BloodDonationSystem.Core.DomainEvents;
using BloodDonationSystem.Core.Events;

namespace BloodDonationSystem.Core.Entities;


    public abstract class BaseEntity : IAggregateRoot
    {
        // public BaseEntity()
        // {
        //     Id = Guid.NewGuid();
        //     CreatedAt = DateTime.Now;
        //     IsDeleted = false;
        // }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; set; }

        public void SetAsDeleted()
        {
            IsDeleted = true;
        }

        protected BaseEntity()
        {
            Id=Guid.NewGuid();
        }

        [NotMapped]
        private readonly List<DomainEvent> _domainEvents = new();
        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        
        public void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
        
    }

 