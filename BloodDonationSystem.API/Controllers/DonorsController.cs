using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.Controllers;

[Route("api/donors")]
[ApiController]

public class DonorsController: ControllerBase
{
    [HttpGet("GetAllDonors")]
    
    public async Task<IActionResult> GetAllDonors()
    {
        return Ok();
    }
    
}