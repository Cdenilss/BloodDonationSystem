using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Commands.DonorsCommand.Insert;

public class CreateDonorHandler : IRequestHandler<CreateDonorCommand, ResultViewModel<Guid>>
{
    private readonly IRepositoryDonor _donorRepository;

    public CreateDonorHandler(IRepositoryDonor donorRepository)
    {
        _donorRepository = donorRepository;
    }

    public async Task<ResultViewModel<Guid>> Handle(CreateDonorCommand request, CancellationToken cancellationToken)
    {
        var donor = request.ToEntity();
        await _donorRepository.Add(donor);
        return ResultViewModel<Guid>.Success(donor.Id);
    }
    
}