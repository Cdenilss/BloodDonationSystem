using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Queries.DonorsQueries.GetByEmail;

public class GetDonorByEmailQueryHandler : IRequestHandler<GetDonorByEmailQuery, ResultViewModel<DonorViewModel>>
{
    private readonly IRepositoryDonor _repository;

    public GetDonorByEmailQueryHandler(IRepositoryDonor repository)
    {
        _repository = repository;
    }

    public async Task<ResultViewModel<DonorViewModel>> Handle(GetDonorByEmailQuery request, CancellationToken cancellationToken)
    {
        var donor = await _repository.GetDonorByEmail(request.Email);
        if (donor == null)
        {
            return ResultViewModel<DonorViewModel>.Error("Doador não encontrado");
        }

        var donorViewModel = DonorViewModel.FromEntity(donor);
        return ResultViewModel<DonorViewModel>.Success(donorViewModel);
    }
}