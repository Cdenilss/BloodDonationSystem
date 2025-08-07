using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Application.Services.ViaCep;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Commands.DonorsCommand.Put;

public class DonorPutCommandHandler : IRequestHandler<DonorPutCommand, ResultViewModel>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IViaCepService _viaCepService;

    public DonorPutCommandHandler(IUnitOfWork unitOfWork, IViaCepService viaCepService)
    {
        _unitOfWork = unitOfWork;
        _viaCepService = viaCepService;
    }


    public async Task<ResultViewModel> Handle(DonorPutCommand request, CancellationToken cancellationToken)
    {
        var donor = await _unitOfWork.Donors.GetDonorByEmail(request.Email);
        if (donor == null)
        {
            return ResultViewModel.Error("Doador não encontrado");
        }

        donor.Update(request.Name, request.Email);

        if (!string.IsNullOrWhiteSpace(request.Cep))
        {
            var addressData = await _viaCepService.GetAddressByCepAsync(request.Cep);
            if (addressData == null || addressData.Erro)
            {
                return ResultViewModel.Error("CEP para atualização não encontrado ou inválido.");
            }


            donor.Address.Update(
                addressData.Logradouro,
                addressData.Localidade,
                addressData.Uf,
                addressData.Cep,
                request.AddressNumber,
                request.AddressComplement,
                addressData.Bairro

            );
        }
        await _unitOfWork.Donors.Update(donor);
        await _unitOfWork.CompleteAsync();
        return ResultViewModel.Success();
    }


}
