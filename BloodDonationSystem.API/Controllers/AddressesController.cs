using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.Controllers;
[Route("api/Andresses")]
[ApiController]

public class AddressesController: ControllerBase

{
    [HttpGet("GetAllAndresses")]
    
    public async Task<IActionResult> GetAllDonors()
    {
        return Ok();
    }
}