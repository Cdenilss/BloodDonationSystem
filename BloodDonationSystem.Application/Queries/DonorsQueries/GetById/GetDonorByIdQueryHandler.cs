using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Queries.DonorsQueries.GetById;

public class GetDonorByIdQueryHandler : IRequestHandler<GetDonorByIdQuery, ResultViewModel<DonorViewModel>>
{
    private readonly IRepositoryDonor _repository;

    public GetDonorByIdQueryHandler(IRepositoryDonor repository)
    {
        _repository = repository;
    }

    public async Task<ResultViewModel<DonorViewModel>> Handle(GetDonorByIdQuery request, CancellationToken cancellationToken)
    {
        var donor = await _repository.GetDetailsById(request.Id);
        if (donor == null)
        {
            return ResultViewModel<DonorViewModel>.Error("Doador não encontrado");
        }

        var donorViewModel = DonorViewModel.FromEntity(donor);
        return ResultViewModel<DonorViewModel>.Success(donorViewModel);
    }
}