using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Application.Validators.DonationValidator;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Commands.DonationsCommand.Insert;

public class CreateDonationCommandHandler : IRequestHandler<CreateDonationCommand, ResultViewModel<Guid>>
{

    private readonly IDonationEligibilityValidator _eligibilityValidator;
    
    private readonly IUnitOfWork _unitOfWork;
    public CreateDonationCommandHandler( IDonationEligibilityValidator eligibilityValidator, IUnitOfWork unitOfWork)
    {
      
        _eligibilityValidator = eligibilityValidator;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResultViewModel<Guid>> Handle(CreateDonationCommand request, CancellationToken cancellationToken)
    {
        
        var donor = await _unitOfWork.Donors.GetDonorByEmail(request.DonorEmail);
        if (donor == null)
        {
            return  ResultViewModel<Guid>.Error("Doador não encontrado");
        }
        var validationResult = await _eligibilityValidator.ValidateAsync(donor, request.DateDonation);
        
        if (!validationResult.IsSuccess)
        {
            
            return ResultViewModel<Guid>.Error(validationResult.Errors);
        }
       
        var bloodStock =
            await _unitOfWork.BloodStocks.GetByTypeAsync(donor.BloodType, donor.RhFactor);
        
            if (bloodStock != null)
            {
                bloodStock.QuantityMl += request.QuantityMl;
              _unitOfWork.BloodStocks.UpdateAsync(bloodStock);
            }
            else
            {
                bloodStock = new BloodStock(donor.BloodType,donor.RhFactor, request.QuantityMl);
                await _unitOfWork.BloodStocks.AddAsync(bloodStock);
                
            }
            var donation = request.ToEntity(donor);
            await _unitOfWork.Donations.Add(donation);
        
        await _unitOfWork.CompleteAsync();
        
        return ResultViewModel<Guid>.Success(donation.Id);
        
    }
}