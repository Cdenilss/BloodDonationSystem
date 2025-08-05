using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;

namespace BloodDonationSystem.Application.Queries.BloodSrocksQueries;

public class GetAllBloodStocksQuery : IRequest<ResultViewModel<List<BloodStockItemViewModel>>>
{
    
}