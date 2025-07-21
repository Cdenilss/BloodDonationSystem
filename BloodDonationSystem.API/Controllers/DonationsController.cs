using BloodDonationSystem.Application.Commands.DonationsCommand.Delete;
using BloodDonationSystem.Application.Commands.DonationsCommand.Insert;
using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Queries.DonationsQueries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.Controllers;
[Route("api/donations")]
[ApiController]

public class DonationsController: ControllerBase
{
    
    private readonly IMediator _mediator;
    public DonationsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("GetAllDonations")]
    
    public async Task<IActionResult> GetAllDonors()
    {
        
        var result = await _mediator.SendWithResponse( new GetAllDonationsQuery());
        
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
        
        return CreatedAtAction(nameof(GetAllDonors), new { id = result.Data}, command);
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
}