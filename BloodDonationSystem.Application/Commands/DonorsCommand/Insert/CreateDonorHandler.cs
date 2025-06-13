using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Mappings;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Application.Services.ViaCep;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Commands.DonorsCommand.Insert;

public class CreateDonorHandler : IRequestHandler<CreateDonorCommand, ResultViewModel<Guid>>
{
    private readonly IRepositoryDonor _donorRepository;
    private readonly IViaCepService _viaCepService;

    public CreateDonorHandler(IRepositoryDonor donorRepository, IViaCepService viaCepService)
    {
        _donorRepository = donorRepository;
        _viaCepService = viaCepService;
    }

    public async Task<ResultViewModel<Guid>> Handle(CreateDonorCommand request, CancellationToken cancellationToken)
    {
        // 1. Chamar o serviço ViaCEP
        var addressData = await _viaCepService.GetAddressByCepAsync(request.Cep);
        if (addressData == null || addressData.Erro)
        {
            return ResultViewModel<Guid>.Error("CEP não encontrado ou inválido.");
        }

        var address = addressData.ToEntity(
            request.AddressNumber, 
             request.AddressComplement
        );
     
        var donor = request.ToEntity(address);
        await _donorRepository.Add(donor);
        return ResultViewModel<Guid>.Success(donor.Id);
    }
}

