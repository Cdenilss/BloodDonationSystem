using BloodDonationSystem.Application.Commands.DonorsCommand.Delete;
using BloodDonationSystem.Application.Commands.DonorsCommand.Insert;
using BloodDonationSystem.Application.Commands.DonorsCommand.Put;
using BloodDonationSystem.Application.Common.Mediator;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Application.Models.ResultViewModel;
using BloodDonationSystem.Application.Queries.DonorsQueries.GetAll;
using BloodDonationSystem.Application.Queries.DonorsQueries.GetByEmail;
using BloodDonationSystem.Application.Queries.DonorsQueries.GetById;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.Controllers;

[Route("api/donors")]
[ApiController]

public class DonorsController: ControllerBase
{
    private readonly IMediator _mediator;
    public DonorsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("GetAllDonors")]
    
    public async Task<IActionResult> GetAllDonors()
    {
        var result = await _mediator.SendWithResponse(new GetAllDonorQuery());
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result);
    }
    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDonorById(Guid id)
    {
      
        var result = await _mediator.SendWithResponse(new GetDonorByIdQuery(id));

        if (!result.IsSuccess)
        {
            return BadRequest();
        }
            return Ok(result);
    }
    
    [HttpGet("GetDonorByEmail")]
    public async Task<IActionResult> GetDonorByEmail(string email)
    {
        var result = await _mediator.SendWithResponse(new GetDonorByEmailQuery(email));

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(result);
    }
    
    
    
    [HttpPost]
    public async Task<IActionResult> AddDonor(CreateDonorCommand command)
    {
        var result = await _mediator.SendWithResponse(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        
        return CreatedAtAction(nameof(GetDonorById), new { id = result.Data }, command);
    }
    
    [HttpPut]
    
    public async Task<IActionResult> UpdateDonor(DonorPutCommand command)
    {
        var result = await _mediator.SendWithResponse(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        
        return NoContent();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteDonor( string email)
    {
        var result = await _mediator.SendWithResponse(new DeleteDonorCommand(email));

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        
        return NoContent();
        
        
    }
}

