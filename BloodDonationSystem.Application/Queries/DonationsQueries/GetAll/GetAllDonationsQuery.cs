using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;

namespace BloodDonationSystem.Application.Queries.DonationsQueries.GetAll;

public class GetAllDonationsQuery : IRequest<ResultViewModel<List<DonationItemViewModel>>>
{
    
}