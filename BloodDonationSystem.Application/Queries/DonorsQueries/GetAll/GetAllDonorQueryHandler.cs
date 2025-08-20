using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Queries.DonorsQueries.GetAll;

public class GetAllDonorQueryHandler : IRequestHandler<GetAllDonorQuery, ResultViewModel<List<DonorItemViewModel>>>
{
    private readonly IDonorRepository _donorRepository;

    public GetAllDonorQueryHandler(IDonorRepository donorRepository)
    {
        _donorRepository = donorRepository;
    }

    public async Task<ResultViewModel<List<DonorItemViewModel>>> Handle(GetAllDonorQuery request,
        CancellationToken cancellationToken)
    {
        var donors = await _donorRepository.GetAll();

        if (!donors.Any())
        {
            return ResultViewModel<List<DonorItemViewModel>>.Error("Lista de Doadores Vazia");
        }

        var model = donors.Select(d => DonorItemViewModel.FromEntity(d)).ToList();

        return ResultViewModel<List<DonorItemViewModel>>.Success(model);
    }
}