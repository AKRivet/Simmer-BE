using Microsoft.AspNetCore.Mvc;

namespace SimmerBackend.Api.Controllers;

/// <summary>Liveness and readiness probes.</summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class HealthController : ControllerBase
{
    /// <summary>Returns the current health status of the service.</summary>
    /// <returns>An object containing the health status and server timestamp.</returns>
    /// <response code="200">Service is healthy.</response>
    [HttpGet]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(new HealthResponse("healthy", DateTime.UtcNow));
    }
}

/// <summary>Health check response payload.</summary>
/// <param name="Status">Textual health status (e.g. "healthy").</param>
/// <param name="Timestamp">UTC timestamp at time of check.</param>
public sealed record HealthResponse(string Status, DateTime Timestamp);
