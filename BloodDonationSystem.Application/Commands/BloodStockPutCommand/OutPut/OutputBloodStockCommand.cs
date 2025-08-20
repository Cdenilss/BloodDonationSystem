using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Application.Commands.BloodStockPutCommand.OutPut;

public class OutputBloodStockCommand : IRequest<ResultViewModel>
{
    public BloodTypeEnum BloodType { get; set; }
    public RhFactorEnum RhFactor { get; set; }
    public int QuantityMl { get; set; }
}