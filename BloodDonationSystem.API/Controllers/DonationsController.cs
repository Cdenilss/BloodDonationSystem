using BloodDonationSystem.Application.Commands.DonationsCommand.Delete;
using BloodDonationSystem.Application.Commands.DonationsCommand.Insert;
using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Queries.DonationsQueries.GellAllLast30DaysDonations;
using BloodDonationSystem.Application.Queries.DonationsQueries.GetAll;
using BloodDonationSystem.Core.Repositories;
using BloodDonationSystem.Infrastructure.Reports;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace BloodDonationSystem.Controllers;

[Route("api/donations")]
[ApiController]
public class DonationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDonationRepository _donationRepository;

    public DonationsController(IMediator mediator, IDonationRepository donationRepository)
    {
        _mediator = mediator;
        _donationRepository = donationRepository;
    }

    [HttpGet("GetAllDonations")]
    public async Task<IActionResult> GetAllDonations()
    {
        var result = await _mediator.SendWithResponse(new GetAllDonationsQuery());

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Data);
    }


    [HttpPost]
    public async Task<IActionResult> AddDonation(CreateDonationCommand command)
    {
        var result = await _mediator.SendWithResponse(command);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetAllDonations), new { id = result.Data }, command);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDonation(Guid id)
    {
        var command = new DeleteDonationCommand(id);
        var result = await _mediator.SendWithResponse(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }

    [HttpGet("reports/donations/last30days")]
    public async Task<IActionResult> GetReportOf30Days()
    {
        var result = await _mediator.SendWithResponse(new GetAllLast30DaysQuery());
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }


        var report = new LastDonationsDetails(result.Data);
        var pdf = report.GeneratePdf(); // byte[]
        return File(pdf, "application/pdf", "donations.pdf");
    }
}