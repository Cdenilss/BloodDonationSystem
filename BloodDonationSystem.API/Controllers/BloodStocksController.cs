using BloodDonationSystem.Application.Commands.BloodStockPutCommand.OutPut;
using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Queries.BloodSrocksQueries;
using BloodDonationSystem.Core.Enum;
using Microsoft.AspNetCore.Mvc;

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
    }


