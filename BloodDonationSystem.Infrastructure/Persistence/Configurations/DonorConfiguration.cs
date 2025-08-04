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

        builder.Property(d => d.Name)
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
            .HasDefaultValueSql("GETUTCDATE()");
        ;

        builder.HasMany(d => d.Donations)
            .WithOne(d => d.Donor)
            .HasForeignKey(d => d.DonorId);

        builder.OwnsOne(d => d.Address, a =>
        {
            a.Property(ad => ad.Street)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Street");

            a.Property(ad => ad.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("City");

            a.Property(ad => ad.State)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("State");

            a.Property(ad => ad.ZipCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("ZipCode");
            a.Property(ad => ad.Number)
                .HasMaxLength(20)
                .HasColumnName("Number");
            a.Property(ad => ad.Complement)
                .HasMaxLength(100)
                .HasColumnName("Complement");
            a.Property(ad => ad.District)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("District");
        });
    }
}