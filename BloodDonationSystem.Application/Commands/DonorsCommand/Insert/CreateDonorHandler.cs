using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Mappings;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Application.Services.ViaCep;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Commands.DonorsCommand.Insert;

public class CreateDonorHandler : IRequestHandler<CreateDonorCommand, ResultViewModel<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IViaCepService _viaCepService;

    public CreateDonorHandler(IUnitOfWork unitOfWork, IViaCepService viaCepService)
    {
        _unitOfWork = unitOfWork;
        _viaCepService = viaCepService;
    }


    public async Task<ResultViewModel<Guid>> Handle(CreateDonorCommand request, CancellationToken cancellationToken)
    {
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
        await _unitOfWork.Donors.Add(donor);
        await _unitOfWork.CompleteAsync();
        return ResultViewModel<Guid>.Success(donor.Id);
    }
}