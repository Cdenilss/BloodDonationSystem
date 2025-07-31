using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Core.Entities;

public class BloodStock: BaseEntity
{
    public BloodStock(BloodTypeEnum bloodType, RhFactorEnum rhFactor, int quantityMl)
    {
        BloodType = bloodType;
        RhFactor = rhFactor;
        QuantityMl = quantityMl;
    }
    
    public BloodTypeEnum BloodType { get; private set; }
    public RhFactorEnum RhFactor { get; private set; }
    public int QuantityMl { get; set;}
    public int MinimumSafeQuantity { get; set; } 
    
    
    // public void InputBloodStock(int quantity)
    // {
    //     QuantityMl += quantity;
    // }
    //
    // public void OutputBloodStock(int quantity)
    // {
    //     QuantityMl -= quantity;
    // }
}

