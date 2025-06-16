using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;

namespace BloodDonationSystem.Application.Queries.DonorsQueries.GetByEmail;

public class GetDonorByEmailQuery : IRequest<ResultViewModel<DonorViewModel>>
{
    public GetDonorByEmailQuery(string email)
    {
        Email = email;
    }

    public string Email { get; private set; }
    
}