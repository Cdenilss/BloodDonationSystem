using BloodDonationSystem.Application.Commands.DonationsCommand.Insert;
using BloodDonationSystem.Core.Repositories;
using FluentValidation;

namespace BloodDonationSystem.Application.Validators.DonationValidator;

public class CreateDonationValidator : AbstractValidator<CreateDonationCommand>
{
    public CreateDonationValidator()
    {
        RuleFor(cmd => cmd.DonorEmail)
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(cmd => cmd.DateDonation)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("A data da doação não pode ser no futuro.")
            .GreaterThanOrEqualTo(DateTime.Today.AddYears(-100)).WithMessage("Data inválida.");

        RuleFor(cmd => cmd.QuantityMl)
            .InclusiveBetween(420, 473).WithMessage("A quantidade de sangue deve estar entre 420ml e 473ml.");
    }
}