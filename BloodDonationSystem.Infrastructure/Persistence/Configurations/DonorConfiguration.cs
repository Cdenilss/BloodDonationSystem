using BloodDonationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloodDonationSystem.Infrastructure.Persistence.Configurations;

public class DonorConfiguration : IEntityTypeConfiguration<Donor>
{
    public void Configure(EntityTypeBuilder<Donor> builder)
    {
        builder.ToTable("Donors");
        builder.HasKey(d => d.Id);
        
        builder.Property(d=>d.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(d => d.Email)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(d => d.Email)
            .IsUnique();

        builder.Property(d => d.BirthDate)
            .IsRequired();

        builder.Property(d => d.Gender)
            .IsRequired();
        builder.Property(d => d.Weight)
            .IsRequired()
            .HasColumnType("DECIMAL(5,2)");
       
        builder.Property(d => d.BloodType)
            .IsRequired()
            .HasMaxLength(3);
        
        builder.Property(d => d.RhFactor)
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(d => d.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");;
        
        builder.HasMany(d => d.Donations)
            .WithOne(d => d.Donor)
            .HasForeignKey(d=>d.DonorId);
        
        builder.HasOne(d => d.Address)
            .WithOne(a => a.Donor)
            .HasForeignKey<Address>(a => a.DonorId) 
            .OnDelete(DeleteBehavior.Cascade);  
    }
}