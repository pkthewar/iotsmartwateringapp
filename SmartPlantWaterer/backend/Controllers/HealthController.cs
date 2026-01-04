using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPlantWaterer.Data;
using SmartPlantWaterer.Models;
using SmartPlantWaterer.Models.DbModels;
using SmartPlantWaterer.Services.Implementations;
using SmartPlantWaterer.Services.Interfaces;

namespace SmartPlantWaterer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController(AppDbContext appDbContext, IHealthService healthService) : ControllerBase
    {
        /// <summary>
        /// Action method which checks whether system is up and running.
        /// </summary>
        /// <returns>StatusCode object with the status code and health details of system.</returns>
        [HttpGet("getHealth")]
        public async Task<IActionResult> Health()
        {
            HealthStatus health = await healthService.CheckAsync();

            int httpStatus = health.OverallStatus!.Equals("Healthy", StringComparison.OrdinalIgnoreCase) ? StatusCodes.Status200OK : StatusCodes.Status503ServiceUnavailable;

            return StatusCode(httpStatus, health);
        }

        [HttpGet("plants/{plantId}")]
        public async Task<IActionResult> GetPlantHeartbeat(int plantId)
        {
            HeartBeat? lastHeartBeat = await appDbContext.HeartBeatLogs.Where(h => h.PlantId == plantId).OrderByDescending(h => h.CreatedOn).FirstOrDefaultAsync();

            if (lastHeartBeat is null)
                return Ok(new
                {
                    plantId,
                    status = "Offline",
                    mesage = "No heartbeat received",
                    voltage = (float?)null
                });

            string status = lastHeartBeat.Value > 3.3f ? "Healthy" : lastHeartBeat.Value > 3.0f ? "Warning" : "Critical";

            return Ok(new
            {
                plantId,
                Status = status,
                message = lastHeartBeat.Message,
                Voltage = lastHeartBeat.Value,
                LastSeen = lastHeartBeat.CreatedOn
            });
        }
    }
}
