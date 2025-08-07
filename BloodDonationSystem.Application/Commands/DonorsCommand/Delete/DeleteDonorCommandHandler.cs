using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Commands.DonorsCommand.Delete;

public class DeleteDonorCommandHandler : IRequestHandler<DeleteDonorCommand, ResultViewModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDonorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultViewModel> Handle(DeleteDonorCommand request, CancellationToken cancellationToken)
    {
        var donor= await _unitOfWork.Donors.GetDonorByEmail(request.Email);
        if (donor == null)
        {
            return ResultViewModel.Error("Doador não encontrado");
        }

        await _unitOfWork.Donors.Delete(donor.Id);
        await _unitOfWork.CompleteAsync();
       return ResultViewModel.Success();
    }
}