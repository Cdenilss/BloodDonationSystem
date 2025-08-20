using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Application.Validators.DonationValidator;

public interface IDonationEligibilityValidator
{
    Task<ResultViewModel> ValidateAsync(Donor donor, DateTime newDonationDate);
}