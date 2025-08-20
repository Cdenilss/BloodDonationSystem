using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Queries.DonorsQueries.GetByEmail;

public class GetDonorByEmailQueryHandler : IRequestHandler<GetDonorByEmailQuery, ResultViewModel<DonorViewModel>>
{
    private readonly IDonorRepository _donorRepository;

    public GetDonorByEmailQueryHandler(IDonorRepository donorRepository)
    {
        _donorRepository = donorRepository;
    }

    public async Task<ResultViewModel<DonorViewModel>> Handle(GetDonorByEmailQuery request,
        CancellationToken cancellationToken)
    {
        var donor = await _donorRepository.GetDonorByEmail(request.Email);
        if (donor == null)
        {
            return ResultViewModel<DonorViewModel>.Error("Doador não encontrado");
        }

        var donorViewModel = DonorViewModel.FromEntity(donor);
        return ResultViewModel<DonorViewModel>.Success(donorViewModel);
    }
}