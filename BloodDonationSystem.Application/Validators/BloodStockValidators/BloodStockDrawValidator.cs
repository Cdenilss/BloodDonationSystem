using System.Data;
using BloodDonationSystem.Application.Commands.BloodStockPutCommand.OutPut;
using BloodDonationSystem.Core.Repositories;
using FluentValidation;


namespace BloodDonationSystem.Application.Validators.BloodStockValidators;

public class BloodStockDrawValidator: AbstractValidator<OutputBloodStockCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public BloodStockDrawValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        var cascadeMode = CascadeMode.Stop;

        RuleFor(cmd => cmd.QuantityMl)
            .GreaterThan(0).WithMessage("A quantidade de sangue a ser retirada deve ser maior que zero.")
            .LessThanOrEqualTo(500).WithMessage("A quantidade de sangue a ser retirada não pode exceder 500ml.");
        RuleFor(cmd => cmd.BloodType)
            .IsInEnum().WithMessage("O tipo sanguíneo é obrigatório e deve ser válido.");
        RuleFor(cmd => cmd.RhFactor)
            .IsInEnum().WithMessage("O fator Rh é obrigatório e deve ser válido.");
        RuleFor(cmd => cmd).MustAsync(HaveEnoughStockAsync)
            .WithMessage("Estoque insuficiente para o tipo sanguíneo solicitado.");

        
        
    }
    
    private async Task<bool> HaveEnoughStockAsync(OutputBloodStockCommand cmd, CancellationToken cancellationToken)
    {
       var bloodStock = await _unitOfWork.BloodStocks.GetByTypeAsync(cmd.BloodType, cmd.RhFactor);
       var available = bloodStock?.QuantityMl >= cmd.QuantityMl;
        return available;
    }
    
}