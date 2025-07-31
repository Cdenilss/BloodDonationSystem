
using BloodDonationSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace BloodDonationSystem.Infrastructure.Persistence.Configurations;

public class BloodStockConfiguration : IEntityTypeConfiguration<BloodStock>
{
    public void Configure(EntityTypeBuilder<BloodStock> builder)
    {
        builder.ToTable("BloodStocks");
        
            builder.ToTable("BloodStocks");
            
             // Define a chave primária composta
            builder.HasKey(bs => new { bs.BloodType, bs.RhFactor });
        
        
        // Configurações das Propriedades
        builder.Property(bs => bs.BloodType)
            .IsRequired()
            .HasMaxLength(3);
            
        builder.Property(bs => bs.RhFactor)
            .IsRequired()
            .HasMaxLength(8);
            
        builder.Property(bs => bs.QuantityMl)
            .IsRequired()
            .HasPrecision(7, 2) 
            .HasDefaultValue(0);
            
        builder.Property(bs => bs.MinimumSafeQuantity)
            .IsRequired()
            .HasPrecision(7, 2)
            .HasDefaultValue(5000.00);  
        
        // Índices para consultas rápidas
        builder.HasIndex(bs => new { bs.BloodType, bs.RhFactor })
            .IsUnique()
            .HasDatabaseName("IX_BloodStock_Type_Rh");
            
    }
}