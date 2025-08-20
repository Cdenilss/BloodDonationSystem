using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;

namespace BloodDonationSystem.Application.Queries.DonorsQueries.GetAll;

public class GetAllDonorQuery : IRequest<ResultViewModel<List<DonorItemViewModel>>>
{
}