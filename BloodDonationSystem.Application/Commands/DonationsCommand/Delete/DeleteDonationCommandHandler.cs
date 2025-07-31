using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Commands.DonationsCommand.Delete;

public class DeleteDonationCommandHandler : IRequestHandler<DeleteDonationCommand, ResultViewModel>
{
    private readonly IDonationRepository _donationRepository;

    public DeleteDonationCommandHandler(IDonationRepository donationRepository)
    {
        _donationRepository = donationRepository;
    }

    public async Task<ResultViewModel> Handle(DeleteDonationCommand request, CancellationToken cancellationToken)
    {
        var donation = await _donationRepository.GetById(request.Id);
        if (donation == null)
        {
            return ResultViewModel.Error("Donation not found.");
        }

        await _donationRepository.Delete(donation.Id);
        
        return ResultViewModel.Success();
    }
}