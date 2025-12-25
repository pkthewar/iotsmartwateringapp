using Microsoft.AspNetCore.SignalR;
using SmartPlantWaterer.Data;
using SmartPlantWaterer.Hubs;
using SmartPlantWaterer.Models;
using SmartPlantWaterer.Models.DbModels;
using SmartPlantWaterer.Services.Interfaces;

namespace SmartPlantWaterer.Services.Implementations
{
    public class TelemetryService(AppDbContext appDbContext, OnnxPredictionService onnxPredictionService, WateringRuleEngine wateringRuleEngine, PumpService pumpService, IHubContext<TelemetryHub> hubContext) : ITelemetryService
    {
        private readonly AppDbContext db = appDbContext;
        private readonly OnnxPredictionService onnx = onnxPredictionService;
        private readonly WateringRuleEngine rules = wateringRuleEngine;
        private readonly PumpService pump = pumpService;
        private readonly IHubContext<TelemetryHub> hub = hubContext;

        public async Task ProcessTelemetryAsync(TelemetryDto dto, PlantProfile plantProfile)
        {
            float score = onnx.Predict(dto.Moisture, dto.Temperature, dto.Humidity);

            bool shouldWater = rules.ShouldWater(new Telemetry
            {
                Moisture = dto.Moisture,
                Temperature = dto.Temperature,
                Humidity = dto.Humidity
            }, plantProfile);

            if (shouldWater)
                await pump.ActivatePumpAsync(dto.PlantId);

            Telemetry telemetry = new()
            {
                PlantId = dto.PlantId,
                Moisture = dto.Moisture,
                Temperature = dto.Temperature,
                Humidity = dto.Humidity,
                Score = score,
                WaterNow = shouldWater
            };

            db.TelemetryLogs.Add(telemetry);
            await db.SaveChangesAsync();

            //Broadcast to all plants. This is commented for now.
            //await hub.Clients.All.SendAsync("TelemetryUpdate", telemetry);

            //Broadcast to a specific plant
            await hub.Clients.Group($"plant-{telemetry.PlantId}").SendAsync("TelemetryUpdate", telemetry);
        }
    }
}
