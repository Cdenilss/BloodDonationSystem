using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;

namespace BloodDonationSystem.Application.Queries.DonationsQueries.GetById;

public class GetDonationByIdQuery: IRequest<ResultViewModel<DonationViewModel>>
{

    public GetDonationByIdQuery(Guid id)
    {
        id = Id;
    }

    public Guid Id { get; private set; }
    
}