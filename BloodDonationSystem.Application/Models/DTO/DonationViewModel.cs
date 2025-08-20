using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Application.Models.DTO;

public class DonationViewModel
{
    
    public DateTime DateDonation { get; private set; }
    public int QuantityMl { get; private set; }
    public Guid Id { get; private set; }
    public DonorItemViewModel Donor { get; private set; }

    public DonationViewModel(Guid id, DateTime dateDonation, int quantityMl, DonorItemViewModel donor)
    {
        Id = id;
        Donor= donor;
        DateDonation = dateDonation;
        QuantityMl = quantityMl;
    }
    public static DonationViewModel FromEntity(Donation donation)
        => new(donation.Id, donation.DateDonation, donation.QuantityMl, DonorItemViewModel.FromEntity(donation.Donor));

}