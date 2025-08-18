using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Queries.BloodSrocksQueries.BloodStockReports;

public class GetReportQuery: IRequest<ResultViewModel<List<BloodStockItemViewModel>>>
{
    
}

public class GellReportQueryHandler:  IRequestHandler<GetReportQuery, ResultViewModel<List<BloodStockItemViewModel>>>
{
    
    private readonly IUnitOfWork _unitOfWork;

    public GellReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<ResultViewModel<List<BloodStockItemViewModel>>> Handle(GetReportQuery request, CancellationToken cancellationToken)
    {
        var stocks = await _unitOfWork.BloodStocks.GetAllAsync();

        if (stocks == null || !stocks.Any())
        {
            return ResultViewModel<List<BloodStockItemViewModel>>.Error("Lista de Estoques de Sangue Vazia");
        }

        var model = stocks.Select(BloodStockItemViewModel.FromEntity).ToList();

        return ResultViewModel<List<BloodStockItemViewModel>>.Success(model);
    }
}