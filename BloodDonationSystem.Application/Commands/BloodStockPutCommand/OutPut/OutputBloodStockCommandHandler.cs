using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Commands.BloodStockPutCommand.OutPut;

public class OutputBloodStockCommandHandler : IRequestHandler<OutputBloodStockCommand, ResultViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    public OutputBloodStockCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultViewModel> Handle(OutputBloodStockCommand request, CancellationToken cancellationToken)
    {
            
            
        var bloodStock = await _unitOfWork.BloodStocks.GetByTypeAsync(request.BloodType, request.RhFactor);
        if (bloodStock == null)
        {
            return ResultViewModel.Error("Blood stock not found.");
        }
            
        bloodStock.QuantityMl -= request.QuantityMl;
        _unitOfWork.BloodStocks.UpdateAsync(bloodStock);
            
        await _unitOfWork.CompleteAsync();
        return ResultViewModel.Success();
           
    }
}