using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;

namespace BloodDonationSystem.Application.Commands.DonationsCommand.Delete;

public class DeleteDonationCommand : IRequest<ResultViewModel>
{
    public DeleteDonationCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
    
}