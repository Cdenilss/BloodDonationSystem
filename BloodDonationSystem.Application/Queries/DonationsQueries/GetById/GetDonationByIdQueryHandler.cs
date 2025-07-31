using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Queries.DonationsQueries.GetById;

public class GetDonationByIdQueryHandler : IRequestHandler<GetDonationByIdQuery, ResultViewModel<DonationViewModel>>
{
    private readonly IDonationRepository _donationRepository;

    public GetDonationByIdQueryHandler(IDonationRepository donationRepository)
    {
        _donationRepository = donationRepository;
    }

    public async Task<ResultViewModel<DonationViewModel>>  Handle(GetDonationByIdQuery request, CancellationToken cancellationToken)
    {
        var donation = await _donationRepository.GetById(request.Id);
        if (donation == null)
        {
            return ResultViewModel<DonationViewModel>.Error("Donation not found.");
        }
        
        var donationViewModel = DonationViewModel.FromEntity(donation);

        return ResultViewModel<DonationViewModel>.Success(donationViewModel);
    }
}