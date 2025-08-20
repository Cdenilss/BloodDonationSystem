using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Application.Queries.DonationsQueries.GellAllLast30DaysDonations;

public class GetAllLast30DaysQuery : IRequest<ResultViewModel<List<DonationViewModel>>>
{
}