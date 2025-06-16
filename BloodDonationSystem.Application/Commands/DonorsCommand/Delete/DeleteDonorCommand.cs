using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;

namespace BloodDonationSystem.Application.Commands.DonorsCommand.Delete;

public class DeleteDonorCommand : IRequest<ResultViewModel>
{
    public DeleteDonorCommand( string email)
    {
        Email = email;
    }
    public string Email { get; set; }
    
}