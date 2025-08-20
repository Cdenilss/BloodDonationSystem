using BloodDonationSystem.Application.Commands.DonorsCommand.Insert;
using BloodDonationSystem.Core.Repositories;
using FluentValidation;

namespace BloodDonationSystem.Application.Validators.DonorValidator;

public class CreateDonorValidator : AbstractValidator<CreateDonorCommand>
{
    public CreateDonorValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.")
            .MinimumLength(1).WithMessage("Nome deve ter no mínimo 1 caractere.");
        RuleFor(d => d.Weight)
            .GreaterThan(50.0).WithMessage("Para doar, o peso mínimo é 50kg.");

        RuleFor(cmd => cmd.Email)
            .EmailAddress().WithMessage("E-mail inválido.");
        RuleFor(d => d.Cep)
            .NotEmpty().WithMessage("CEP é obrigatório.")
            .Length(8).WithMessage("CEP deve ter 8 dígitos.");
    }

    protected bool BeOfLegalAge(DateTime birthDate)
    {
        return birthDate <= DateTime.Today.AddYears(-18);
    }
}