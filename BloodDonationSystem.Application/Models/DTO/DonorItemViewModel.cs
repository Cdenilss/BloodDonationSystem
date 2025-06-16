using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Application.Models.DTO;

public class DonorItemViewModel
{
    public DonorItemViewModel(Guid id,string name, TypeBloodEnum typeBlood, RhFactorEnum rhFactor)
    {
        Id = id;
        Name = name;
        TypeBlood = typeBlood;
        RhFactor = rhFactor;
        
       
    }
    public Guid Id { get; set; }
    public string Name { get; private set; }
    
    public TypeBloodEnum TypeBlood { get; private set; }
    public RhFactorEnum RhFactor { get; private set; }
    
    public static DonorItemViewModel FromEntity(Donor donor)
        => new(donor.Id,donor.Name, donor.TypeBlood, donor.RhFactor);
}