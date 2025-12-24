using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPlantWaterer.Data;
using SmartPlantWaterer.Models;
using SmartPlantWaterer.Services.Interfaces;

namespace SmartPlantWaterer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelemetryController(AppDbContext appDbContext, ITelemetryService telemetryService, IPumpService pumpService, IOnnxPredictionService onnxPredictionService, IHealthService healthService) : ControllerBase
    {
        private readonly AppDbContext appDbContext = appDbContext;
        private readonly ITelemetryService telemetryService = telemetryService;
        private readonly IPumpService pumpService = pumpService;
        private readonly IOnnxPredictionService onnxPredictionService = onnxPredictionService;
        private readonly IHealthService healthService = healthService;

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

        /// <summary>
        /// Action method which inserts the watering activity of the plant.
        /// </summary>
        /// <param name="telemetryDto">Details of the plant and its surrounding environment</param>
        /// <returns>Ok object which mentions successful insertion of record.</returns>
        [HttpPost("ingest")]
        public async Task<IActionResult> Ingest([FromBody] TelemetryDto telemetryDto)
        {
            if (telemetryDto == null)
                return BadRequest("Telemetry payload is missing");

            await telemetryService.ProcessTelemetryAsync(telemetryDto);

            return Ok(new
            {
                Message = "Telemetry processed"
            });
        }

        /// <summary>
        /// Action method which triggers the watering service to water a particular plant.
        /// </summary>
        /// <param name="plantId">Id of the plant to be watered</param>
        /// <returns>Success message once watering is successfully triggered.</returns>
        [HttpPost("water/{plantId}")]
        public async Task<IActionResult> WaterPlant(int plantId)
        {
            await pumpService.ActivatePumpAsync(plantId);

            return Ok(new
            {
                plantId,
                Message = "Watering triggered manually"
            });
        }

        /// <summary>
        /// Action method which retrieved the latest watering telemetry record for a given plant.
        /// </summary>
        /// <param name="plantId">Id of the plant</param>
        /// <returns>Telemetry object which has the latest watering record.</returns>
        [HttpGet("{plantId}/latest")]
        public async Task<IActionResult> GetLatest(int plantId)
        {
            Telemetry? telemetry = await appDbContext.TelemetryLogs.OrderByDescending(t => t.CreatedAt).FirstOrDefaultAsync(t => t.PlantId == plantId);

            if (telemetry == null)
                return NotFound("No telemetry found");

            return Ok(telemetry);
        }

        /// <summary>
        /// Action method which retrieves the watering history of a given plant given a time period.
        /// </summary>
        /// <param name="plantId">Id of the plant</param>
        /// <param name="hrs">No. of hours before which logs must be fetched from DB. Default is 24 hrs.</param>
        /// <returns>List containing watering history of the plant over a given period of time.</returns>
        [HttpGet("{plantId}/history")]
        public async Task<IActionResult> History(int plantId, [FromQuery] int hrs = 24)
        {
            DateTime since = DateTime.UtcNow.AddHours(hrs);

            List<Telemetry> histories = await appDbContext.TelemetryLogs.Where(t => t.PlantId == plantId && t.CreatedAt >= since).OrderBy(t => t.CreatedAt).ToListAsync();

            return Ok(histories);
        }

        /// <summary>
        /// Action Method which predicts whether watering is needed for a given plant.
        /// </summary>
        /// <param name="telemetryDto">Details of the plant and its surrounding environment</param>
        /// <returns>Prediction result.</returns>
        [HttpPost("predict")]
        public IActionResult Predict([FromBody] TelemetryDto telemetryDto)
        {
            if (telemetryDto == null)
                return BadRequest();

            var score = onnxPredictionService.Predict(telemetryDto.Moisture, telemetryDto.Temperature, telemetryDto.Humidity);

            return Ok(new
            {
                telemetryDto.PlantId,
                score,
                WaterRecommended = score > 0.5f
            });
        }
    }
}
