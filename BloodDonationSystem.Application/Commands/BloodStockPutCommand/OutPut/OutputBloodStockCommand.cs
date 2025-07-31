using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.ResultViewModel;

namespace BloodDonationSystem.Application.Commands.BloodStockPutCommand.OutPut;

public class OutputBloodStockCommand : IRequest<ResultViewModel>
{
    Guid Id { get; set; }
}

//
// public class OutputBloodStockCommandHandler : IRequestHandler<OutputBloodStockCommand, ResultViewModel>
// {
//     // public Task<ResultViewModel> Handle(OutputBloodStockCommand request, CancellationToken cancellationToken)
//     // {
//     // }