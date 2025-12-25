using Microsoft.EntityFrameworkCore;
using MQTTnet;
using SmartPlantWaterer.Data;
using SmartPlantWaterer.Models;
using SmartPlantWaterer.Services.Interfaces;

namespace SmartPlantWaterer.Services.Implementations
{
    public class HealthService(AppDbContext appDbContext, IOnnxPredictionService onnxPredictionService) : IHealthService
    {
        private readonly AppDbContext appDbContext = appDbContext;
        private readonly IOnnxPredictionService onnxPredictionService = onnxPredictionService;

        public async Task<HealthStatus> CheckAsync()
        {
            HealthStatus healthStatus = new()
            {
                IsApiRunning = true
            };

            try
            {
                healthStatus.IsDatabaseRunning = await appDbContext.Database.CanConnectAsync();
            }
            catch
            {
                healthStatus.IsDatabaseRunning = false;
            }

            try
            {
                IMqttClient client = new MqttClientFactory().CreateMqttClient();

                await client.ConnectAsync(new MqttClientOptionsBuilder().WithTcpServer("localhost").WithTimeout(TimeSpan.FromSeconds(5)).Build());

                healthStatus.IsMqttWorking = client.IsConnected;

                await client.DisconnectAsync();
            }
            catch
            {
                healthStatus.IsMqttWorking = false;
            }

            try
            {
                float score = onnxPredictionService.Predict(300, 25, 50);

                healthStatus.IsOnnxPredicting = !float.IsNaN(score);
            }
            catch
            {
                healthStatus.IsOnnxPredicting = false;
            }

            try
            {
                DateTime mostRecentTelemetry = await appDbContext.TelemetryLogs.OrderByDescending(t => t.CreatedOn).Select(t => t.CreatedOn).FirstOrDefaultAsync();

                healthStatus.IsTelemetryFresh = mostRecentTelemetry > DateTime.UtcNow.AddMinutes(-5);
            }
            catch
            {
                healthStatus.IsTelemetryFresh = false;
            }

            healthStatus.OverallStatus = healthStatus.IsApiRunning && healthStatus.IsDatabaseRunning && healthStatus.IsMqttWorking && healthStatus.IsOnnxPredicting && healthStatus.IsTelemetryFresh ? "Healthy" : "Degraded";

            return healthStatus;
        }
    }
}
