using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Application.Validators.DonationValidator;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Commands.DonationsCommand.Insert;

public class CreateDonationCommandHandler : IRequestHandler<CreateDonationCommand, ResultViewModel<Guid>>
{
    private readonly IRepositoryDonation _donationRepository;
    private readonly IDonationEligibilityValidator _eligibilityValidator;
    
    private readonly IRepositoryDonor _donorRepository;
    public CreateDonationCommandHandler(IRepositoryDonation donationRepository, IRepositoryDonor donorRepository, IDonationEligibilityValidator eligibilityValidator)
    {
        _donationRepository = donationRepository;
        _donorRepository = donorRepository;
        _eligibilityValidator = eligibilityValidator;
    }
    
    public async Task<ResultViewModel<Guid>> Handle(CreateDonationCommand request, CancellationToken cancellationToken)
    {
        var donor = await _donorRepository.GetDonorByEmail(request.DonorEmail);
        await _donorRepository.GetById(donor.Id);
        if (donor == null)
        {
            return  ResultViewModel<Guid>.Error("Doador não encontrado");
        }
        var validationResult = await _eligibilityValidator.ValidateAsync(donor, request.DateDonation);
        
        if (!validationResult.IsSuccess)
        {
            
            return ResultViewModel<Guid>.Error(validationResult.Errors);
        }
        var donation = request.ToEntity(donor);
        
        await _donationRepository.Add(donation);
        
        return ResultViewModel<Guid>.Success(donation.Id);
        
    }
}