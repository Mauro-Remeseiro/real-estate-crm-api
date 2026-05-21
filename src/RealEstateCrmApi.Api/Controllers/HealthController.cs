using Microsoft.AspNetCore.Mvc;

namespace RealEstateCrmApi.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() => Ok("OK");
}
