using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.Controllers;
[Route("api/donations")]
[ApiController]

public class DonationsController: ControllerBase
{
    [HttpGet("GetAllDonations")]
    
    public async Task<IActionResult> GetAllDonors()
    {
        return Ok();
    }
}