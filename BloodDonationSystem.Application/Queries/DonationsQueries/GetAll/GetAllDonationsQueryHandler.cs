using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Queries.DonationsQueries.GetAll;

public class
    GetAllDonationsQueryHandler : IRequestHandler<GetAllDonationsQuery, ResultViewModel<List<DonationItemViewModel>>>
{
    private readonly IDonationRepository _donationRepository;

    public GetAllDonationsQueryHandler(IDonationRepository donationRepository)
    {
        _donationRepository = donationRepository;
    }

    public async Task<ResultViewModel<List<DonationItemViewModel>>> Handle(GetAllDonationsQuery request,
        CancellationToken cancellationToken)
    {
        var donations = await _donationRepository.GetAll();

        if (!donations.Any())
        {
            return ResultViewModel<List<DonationItemViewModel>>.Error("Lista de Doações Vazia");
        }

        var model = donations.Select(d => DonationItemViewModel.FromEntity(d)).ToList();

        return ResultViewModel<List<DonationItemViewModel>>.Success(model);
    }
}