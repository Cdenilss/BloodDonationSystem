using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Application.Models.DTO;

public class DonorItemViewModel
{
    public DonorItemViewModel(Guid id, string name, BloodTypeEnum bloodType, RhFactorEnum rhFactor)
    {
        Id = id;
        Name = name;
        BloodType = bloodType;
        RhFactor = rhFactor;
    }

    public Guid Id { get; set; }
    public string Name { get; private set; }

    public BloodTypeEnum BloodType { get; private set; }
    public RhFactorEnum RhFactor { get; private set; }

    public static DonorItemViewModel FromEntity(Donor donor)
        => new(donor.Id, donor.Name, donor.BloodType, donor.RhFactor);
}