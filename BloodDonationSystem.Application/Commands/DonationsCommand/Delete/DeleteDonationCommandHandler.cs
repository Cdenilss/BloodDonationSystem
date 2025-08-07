using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Commands.DonationsCommand.Delete;

public class DeleteDonationCommandHandler : IRequestHandler<DeleteDonationCommand, ResultViewModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDonationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultViewModel> Handle(DeleteDonationCommand request, CancellationToken cancellationToken)
    {
        var donation = await _unitOfWork.Donations.GetById(request.Id);
        if (donation == null)
        {
            return ResultViewModel.Error("Donation not found.");
        }

        await _unitOfWork.Donations.Delete(donation.Id);
        await _unitOfWork.CompleteAsync();

        return ResultViewModel.Success();
    }
}