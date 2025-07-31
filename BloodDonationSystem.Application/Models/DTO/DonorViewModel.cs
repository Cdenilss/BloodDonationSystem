using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Application.Models.DTO;

public class DonorViewModel
{
    public DonorViewModel(Guid id,string name, string email, DateTime birthDate, GenderEnum gender, double weight, BloodTypeEnum bloodType, RhFactorEnum rhFactor, List<DonationViewModel> donations, AddressViewModel address)
    {
        Id = id;
        Name = name;
        Email = email;
        BirthDate = birthDate;
        Gender = gender;
        Weight = weight;
        BloodType = bloodType;
        RhFactor = rhFactor;
        Donations = donations;
        Address = address;
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public DateTime BirthDate { get; private set; }
    public GenderEnum Gender { get; private set; }
    public double Weight { get; private set; }
    public BloodTypeEnum BloodType { get; private set; }
    public RhFactorEnum RhFactor { get; private set; }
    public List<DonationViewModel>? Donations { get; private set; }
    public AddressViewModel Address { get; private set; }
    
    public static DonorViewModel FromEntity(Donor donor)
    {
        
        var donationViewModels = donor.Donations
            .Select(donation => new DonationViewModel(donation.Id, donation.DateDonation, donation.QuantityMl, null!)) // Passando null! para o DonorItemViewModel por enquanto
            .ToList();
        
        var addressViewModel = AddressViewModel.FromEntity(donor.Address);
        return new DonorViewModel(
            donor.Id,
            donor.Name,
            donor.Email,
            donor.BirthDate,
            donor.Gender,
            donor.Weight,
            donor.BloodType,
            donor.RhFactor,
            // $"{donor.TypeBlood}{donor.RhFactor}", 
            donationViewModels,
            addressViewModel
        );
    }
}