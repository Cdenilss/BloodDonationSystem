using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Application.Commands.DonorsCommand.Put;

public class DonorPutCommand : IRequest<ResultViewModel>
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Cep { get; set; }
    public string AddressNumber { get; set; }
    public string? AddressComplement { get; set; }

    public AddressViewModel Address { get; private set; }
}