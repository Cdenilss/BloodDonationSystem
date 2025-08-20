using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using System;

namespace BloodDonationSystem.Application.Queries.DonorsQueries.GetById;
// Verifique se este namespace corresponde à sua pasta

public class GetDonorByIdQuery : IRequest<ResultViewModel<DonorViewModel>>
{
    public GetDonorByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}