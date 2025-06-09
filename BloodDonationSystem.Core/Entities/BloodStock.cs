namespace BloodDonationSystem.Core.Entities;

public class BloodStock: BaseEntity
{
    public BloodStock(string bloodType, string rhFactor, int quantityMl)
    {
        BloodType = bloodType;
        RhFactor = rhFactor;
        QuantityMl = quantityMl;
    }
    
    public string BloodType { get; private set; }
    public string RhFactor { get; private set; }
    public int QuantityMl { get; private set;}
    public int MinimumSafeQuantity { get; private set; } 
}