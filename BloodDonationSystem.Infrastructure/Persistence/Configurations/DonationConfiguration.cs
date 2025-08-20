using BloodDonationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloodDonationSystem.Infrastructure.Persistence.Configurations;

public class DonationConfiguration : IEntityTypeConfiguration<Donation>
{
    public void Configure(EntityTypeBuilder<Donation> builder)
    {
        builder.ToTable("Donations");
        builder.HasKey(dd => dd.Id);

        builder.Property(dd => dd.DateDonation)
            .IsRequired();
        builder.Property(dd => dd.QuantityMl)
            .IsRequired();

        builder.Property(dd => dd.DonorId)
            .IsRequired();

        builder.HasOne(d => d.Donor)
            .WithMany(d => d.Donations)
            .HasForeignKey(d => d.DonorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}