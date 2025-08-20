using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Application.Models.DTO;

public class DonationItemViewModel
{
    public DateTime DateDonation { get; private set; }
    public int QuantityMl { get; private set; }
    public Guid Id { get; private set; }

    public RhFactorEnum RhFactor { get; private set; }

    public BloodTypeEnum BloodType { get; private set; }


    public static DonationItemViewModel FromEntity(Donation donation)
    {
        return new DonationItemViewModel
        {
            Id = donation.Id,
            DateDonation = donation.DateDonation,
            QuantityMl = donation.QuantityMl,
            RhFactor = donation.Donor.RhFactor,
            BloodType = donation.Donor.BloodType
        };
    }
}