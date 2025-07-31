using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Validators.DonationValidator;

public class DonationEligibilityValidator: IDonationEligibilityValidator
{
    private readonly IDonationRepository _donationRepository;

    public DonationEligibilityValidator(IDonationRepository donationRepository)
    {
        _donationRepository = donationRepository;
    }

    public async Task<ResultViewModel> ValidateAsync(Donor donor, DateTime newDonationDate)
    {
        var ageValidationResult = ValidateDonorAge(donor);
        if (!ageValidationResult.IsSuccess) return ageValidationResult;

        var intervalValidationResult = await ValidateDonationInterval(donor, newDonationDate);
        if (!intervalValidationResult.IsSuccess) return intervalValidationResult;

        return ResultViewModel.Success();
    }

    private ResultViewModel ValidateDonorAge(Donor donor)
    {
        if (donor.BirthDate > DateTime.Today.AddYears(-18))
        {
            return ResultViewModel.Error("Doador é menor de idade e não pode realizar uma doação.");
        }
        return ResultViewModel.Success();
    }

    private async Task<ResultViewModel> ValidateDonationInterval(Donor donor, DateTime newDonationDate)
    {
        var lastDonation = await _donationRepository.GetLastByDonorIdAsync(donor.Id);
        if (lastDonation == null) return ResultViewModel.Success();

        var requiredInterval = donor.Gender == GenderEnum.Male ? 60 : 90;
        var daysSinceLastDonation = (newDonationDate.Date - lastDonation.DateDonation.Date).TotalDays;

        if (daysSinceLastDonation < requiredInterval)
        {
            var nextDate = lastDonation.DateDonation.AddDays(requiredInterval);
            return ResultViewModel.Error($"Intervalo mínimo para doação não foi atingido. Próxima doação possível a partir de {nextDate:dd-MM-yyyy}.");
        }
        
        return ResultViewModel.Success();
    }
}