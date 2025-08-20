using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Application.Commands.DonationsCommand.Insert;

public class CreateDonationCommand : IRequest<ResultViewModel<Guid>>
{
    public string DonorEmail { get; set; }
    public DateTime DateDonation { get; set; }
    public int QuantityMl { get; set; }


    public Donation ToEntity(Donor donor)
        => new(donor.Id, DateDonation, QuantityMl);
}