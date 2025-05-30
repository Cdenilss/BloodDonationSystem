namespace BloodDonationSystem.Core.Entities;

public class BloodStock: BaseEntity
{
    public BloodStock(string bloodType, string rhFactor, int quantityMl)
    {
        BloodType = bloodType;
        RhFactor = rhFactor;
        QuantityMl = quantityMl;
    }
    
    public string BloodType { get; set; }
    public string RhFactor { get; set; }
    public int QuantityMl { get; set; }
    public int MinimumSafeQuantity { get; set; } // ser
}