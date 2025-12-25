using SmartPlantWaterer.Models.DbModels;
using SmartPlantWaterer.Services.Interfaces;

namespace SmartPlantWaterer.Services.Implementations
{
    public class AlertService(IEnumerable<IAlertChannel> alertChannels) : IAlertService
    {
        private readonly IEnumerable<IAlertChannel> alertChannels = alertChannels;

        public async Task CheckAsync(Telemetry telemetry)
        {
            if (telemetry.Moisture < 200)
                await SendAlert("Soil too dry!");

            if (telemetry.Temperature > 45)
                await SendAlert("Temperature critical");
        }

        private async Task SendAlert(string msg)
        {
            foreach (IAlertChannel channel in alertChannels)
                await channel.SendAlert(msg);
        }
    }
}
