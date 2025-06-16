using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Commands.DonorsCommand.Delete;

public class DeleteDonorCommandHandler : IRequestHandler<DeleteDonorCommand, ResultViewModel>
{
    private readonly IRepositoryDonor _donorRepository;

    public DeleteDonorCommandHandler(IRepositoryDonor donorRepository)
    {
        _donorRepository = donorRepository;
    }

    public async Task<ResultViewModel> Handle(DeleteDonorCommand request, CancellationToken cancellationToken)
    {
        var donor= await _donorRepository.GetDonorByEmail(request.Email);   
        if (donor == null)
        {
            return ResultViewModel.Error("Doador não encontrado");
        }
        
        await _donorRepository.Delete(donor.Id);
       return ResultViewModel.Success();
    }
}