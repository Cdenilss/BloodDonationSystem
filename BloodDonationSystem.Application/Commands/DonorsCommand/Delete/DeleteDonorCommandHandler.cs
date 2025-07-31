using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Commands.DonorsCommand.Delete;

public class DeleteDonorCommandHandler : IRequestHandler<DeleteDonorCommand, ResultViewModel>
{
    private readonly IDonorRepository _donorRepository;

    public DeleteDonorCommandHandler(IDonorRepository donorRepository)
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