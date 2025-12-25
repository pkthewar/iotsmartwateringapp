using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using SmartPlantWaterer.Data;
using SmartPlantWaterer.Models;
using SmartPlantWaterer.Models.DbModels;
using SmartPlantWaterer.Services.Interfaces;

namespace SmartPlantWaterer.Services.Implementations
{
    public class HeartbeatService(AppDbContext appDbContext, IEnumerable<IAlertChannel> alertChannels) : IHeartbeatService
    {
        private readonly AppDbContext appDbContext = appDbContext;

        private readonly IEnumerable<IAlertChannel> alertChannels = alertChannels;

        public async Task ProcessHeartbeatAsync(HeartBeatDto heartBeatDto)
        {
            TimeSpan coolDown = TimeSpan.FromMinutes(30);

            bool recentHeartbeatExists = await appDbContext.HeartBeatLogs.AnyAsync(h => h.PlantId == heartBeatDto.PlantId && h.Type == "LowBattery" && h.CreatedOn > DateTime.UtcNow - coolDown);

            if (recentHeartbeatExists)
                return;

            string message = $"Low Battery detected! Plant: {heartBeatDto.PlantId} & Voltage: {heartBeatDto.BatteryVoltage:F2}V";

            HeartBeat heartBeatLog = new()
            {
                Id = Guid.NewGuid(),
                PlantId = heartBeatDto.PlantId,
                Type = "LowBattery",
                Message = message,
                Value = heartBeatDto.BatteryVoltage,
                CreatedOn = DateTime.UtcNow,
                Acknowledged = false
            };

            appDbContext.HeartBeatLogs.Add(heartBeatLog);
            await appDbContext.SaveChangesAsync();

            foreach (IAlertChannel alertChannel in alertChannels)
                await alertChannel.SendAlert(message);

            await Task.CompletedTask;
        }
    }
}
