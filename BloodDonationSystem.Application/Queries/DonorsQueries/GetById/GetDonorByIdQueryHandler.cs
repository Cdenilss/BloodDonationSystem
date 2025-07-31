using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Queries.DonorsQueries.GetById;

public class GetDonorByIdQueryHandler : IRequestHandler<GetDonorByIdQuery, ResultViewModel<DonorViewModel>>
{
    private readonly IDonorRepository _donorRepository;

    public GetDonorByIdQueryHandler(IDonorRepository donorRepository)
    {
        _donorRepository = donorRepository;
    }

    public async Task<ResultViewModel<DonorViewModel>> Handle(GetDonorByIdQuery request, CancellationToken cancellationToken)
    {
        var donor = await _donorRepository.GetDetailsById(request.Id);
        if (donor == null)
        {
            return ResultViewModel<DonorViewModel>.Error("Doador não encontrado");
        }

        var donorViewModel = DonorViewModel.FromEntity(donor);
        return ResultViewModel<DonorViewModel>.Success(donorViewModel);
    }
}