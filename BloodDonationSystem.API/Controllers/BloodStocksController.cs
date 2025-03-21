using Microsoft.AspNetCore.Mvc;

namespace BloodDonationSystem.Controllers;

[Route("api/bloodstocks")]
[ApiController]
public class BloodStocksController: ControllerBase
{
    [HttpGet("GetBloodStocks")]
    
    public async Task<IActionResult> GetAllDonors()
    {
        return Ok();
    }
}