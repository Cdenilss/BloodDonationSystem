using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Core.Repositories;

namespace BloodDonationSystem.Application.Queries.DonationsQueries.GellAllLast30DaysDonations;

public class GetAllLast30DaysQueryHandler : IRequestHandler<GetAllLast30DaysQuery, ResultViewModel<List<DonationViewModel>>>
{
    private readonly IDonationRepository _donationRepository;

    public GetAllLast30DaysQueryHandler(IDonationRepository donationRepository)
    {
        _donationRepository = donationRepository;
    }


    public async Task<ResultViewModel<List<DonationViewModel>>> Handle(GetAllLast30DaysQuery request, CancellationToken cancellationToken)
    {
        var donations = await _donationRepository.GetAllLast30DaysDonation();

        if (!donations.Any())
        {
            return ResultViewModel<List<DonationViewModel>>.Error("Lista de Doações Vazia");
        }

        var model = donations.Select(d => DonationViewModel.FromEntity(d)).ToList();
        
        
        //     var report = new LastDonationsDetails(donations);
        //     var pdf = report.GeneratePdf(); // byte[]
        //     return File(pdf, "application/pdf", "donations.pdf");
        
        return ResultViewModel<List<DonationViewModel>>.Success(model);
    }
}