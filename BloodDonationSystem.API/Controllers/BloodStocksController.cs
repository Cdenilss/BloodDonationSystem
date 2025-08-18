using BloodDonationSystem.Application.Commands.BloodStockPutCommand.OutPut;
using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Queries.BloodSrocksQueries;
using BloodDonationSystem.Application.Queries.BloodSrocksQueries.BloodStockReports;
using BloodDonationSystem.Core.Enum;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace BloodDonationSystem.Controllers;

[Route("api/bloodstocks")]
[ApiController]
public class BloodStocksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BloodStocksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetAllBloodStocks")]
    public async Task<IActionResult> GetAllBloodStocks()
    {

        var result = await _mediator.SendWithResponse(new GetAllBloodStocksQuery());
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpPut("bloodDraw")]
        public async Task<IActionResult> BloodDraw(OutputBloodStockCommand command)
        {
            var result = await _mediator.SendWithResponse(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    [HttpGet("GetReportBloodStocks")]
    public async Task<IActionResult> GetReportBloodStocks()
    {

        var result = await _mediator.SendWithResponse(new GetReportQuery());
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        var report = new BloodStockReport(result.Data);
        var pdf = report.GeneratePdf(); // byte[]
        return File(pdf, "application/pdf", "BloodStockReport.pdf");

       
    }
    }



