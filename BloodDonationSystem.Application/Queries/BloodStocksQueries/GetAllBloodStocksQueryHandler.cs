using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Queries.BloodStocksQueries;

public class
    GetAllBloodStocksQueryHandler : IRequestHandler<GetAllBloodStocksQuery,
    ResultViewModel<List<BloodStockItemViewModel>>>
{
    private readonly IBloodStockRepository _bloodStockRepository;

    public GetAllBloodStocksQueryHandler(IBloodStockRepository bloodStockRepository)
    {
        _bloodStockRepository = bloodStockRepository;
    }

    public async Task<ResultViewModel<List<BloodStockItemViewModel>>> Handle(GetAllBloodStocksQuery request,
        CancellationToken cancellationToken)
    {
        var stocks = await _bloodStockRepository.GetAllAsync();

        if (stocks == null || !stocks.Any())
        {
            return ResultViewModel<List<BloodStockItemViewModel>>.Error("Lista de Estoques de Sangue Vazia");
        }

        var model = stocks.Select(BloodStockItemViewModel.FromEntity).ToList();

        return ResultViewModel<List<BloodStockItemViewModel>>.Success(model);
    }
}