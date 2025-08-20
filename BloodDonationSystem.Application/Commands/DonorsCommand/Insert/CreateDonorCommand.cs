using System.Text.Json.Serialization;
using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Application.Commands.DonorsCommand.Insert;

public class CreateDonorCommand : IRequest<ResultViewModel<Guid>>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public GenderEnum Gender { get; set; }
    public double Weight { get; set; }
    public BloodTypeEnum BloodType { get; set; }
    public RhFactorEnum RhFactor { get; set; }

    public string Cep { get; set; }
    public string AddressNumber { get; set; }
    public string? AddressComplement { get; set; }


    public Donor ToEntity(Address address)
        => new(Name, Email, BirthDate, Gender, Weight, BloodType, RhFactor, address);
}